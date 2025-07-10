using Ran.Core.Exceptions;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Extensions.Internal;
using Ran.Core.Logging;
using Ran.Core.Modularity;
using Ran.Core.Utils.System;
using Ran.Core.Utils.Text;

namespace Ran.Core.Application;

/// <summary>
/// 应用基类
/// </summary>
public class ApplicationBase : IApplication
{
    /// <summary>
    /// 应用程序启动(入口)模块的类型
    /// </summary>
    public Type StartupModuleType { get; }

    /// <summary>
    /// 所有服务注册的列表，应用程序初始化后，不能向这个集合添加新的服务
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// 应用程序根服务提供器，在初始化应用程序之前不能使用
    /// </summary>
    public IServiceProvider ServiceProvider { get; private set; } = default!;

    /// <summary>
    /// 模块
    /// </summary>
    public IReadOnlyList<IModuleDescriptor> Modules { get; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string? ApplicationName { get; }

    /// <summary>
    /// 实例 ID
    /// </summary>
    public string InstanceId { get; } = Guid.NewGuid().ToString();

    private bool _configuredServices;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    internal ApplicationBase(Type startupModuleType, IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction)
    {
        _ = CheckHelper.NotNull(startupModuleType, nameof(startupModuleType));
        _ = CheckHelper.NotNull(services, nameof(services));

        // 设置启动模块
        StartupModuleType = startupModuleType;
        Services = services;

        // 添加一个空的对象访问器，该访问器的值会在初始化的时候被赋值
        _ = services.TryAddObjectAccessor<IServiceProvider>();

        // 调用用户传入的配置委托
        ApplicationCreationOptions options = new(services);
        optionsAction?.Invoke(options);

        ApplicationName = GetApplicationName(options);

        // 注册自己
        _ = services.AddSingleton<IApplication>(this);
        _ = services.AddSingleton<IApplicationInfoAccessor>(this);
        _ = services.AddSingleton<IModuleContainer>(this);
        _ = services.AddSingleton<IHostEnvironment>(new HostEnvironment { EnvironmentName = options.Environment });

        // 添加日志等基础设施组件
        services.AddCoreServices();
        // 添加核心的 XiHan 服务，主要是模块系统相关组件
        services.AddCoreServices(this, options);

        // 加载模块，并按照依赖关系排序，依次执行他们的生命周期方法
        Modules = LoadModules(services, options);

        if (!options.SkipConfigureServices)
        {
            ConfigureServices();
        }
    }

    #region 初始化模块

    /// <summary>
    /// 记录初始化日志
    /// </summary>
    /// <param name="serviceProvider"></param>
    private static void WriteInitLogs(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger<ApplicationBase>>();
        if (logger is null)
        {
            return;
        }

        var initLogger = serviceProvider.GetRequiredService<IInitLoggerFactory>().Create<ApplicationBase>();

        foreach (var entry in initLogger.Entries)
        {
            logger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
        }

        initLogger.Entries.Clear();
    }

    /// <summary>
    /// 加载模块
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    private IModuleDescriptor[] LoadModules(IServiceCollection services,
        ApplicationCreationOptions options)
    {
        return services.GetSingletonInstance<IModuleLoader>()
            .LoadModules(services, StartupModuleType, options.PlugInSources);
    }

    /// <summary>
    /// 获取应用程序名称
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private static string? GetApplicationName(ApplicationCreationOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.ApplicationName))
        {
            return options.ApplicationName!;
        }

        var configuration = options.Services.GetConfigurationOrNull();
        if (configuration is not null)
        {
            var appNameConfig = configuration["ApplicationName"];
            if (!string.IsNullOrWhiteSpace(appNameConfig))
            {
                return appNameConfig!;
            }
        }

        var entryAssembly = Assembly.GetEntryAssembly();
        return entryAssembly?.GetName().Name;
    }

    /// <summary>
    /// 尝试设置环境
    /// </summary>
    /// <param name="services"></param>
    private static void TryToSetEnvironment(IServiceCollection services)
    {
        var hostEnvironment = services.GetSingletonInstance<IHostEnvironment>();
        if (hostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
        {
            hostEnvironment.EnvironmentName = Environments.Production;
        }
    }

    /// <summary>
    /// 设置服务提供器
    /// </summary>
    /// <param name="serviceProvider"></param>
    protected void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
    }

    /// <summary>
    /// 初始化模块，异步
    /// </summary>
    /// <returns></returns>
    protected async Task InitializeModulesAsync()
    {
        using var scope = ServiceProvider.CreateScope();
        WriteInitLogs(scope.ServiceProvider);
        await scope.ServiceProvider.GetRequiredService<IModuleManager>()
            .InitializeModulesAsync(new ApplicationInitializationContext(scope.ServiceProvider));
    }

    /// <summary>
    /// 初始化模块
    /// </summary>
    protected void InitializeModules()
    {
        using var scope = ServiceProvider.CreateScope();
        WriteInitLogs(scope.ServiceProvider);
        scope.ServiceProvider.GetRequiredService<IModuleManager>()
            .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
    }

    #endregion 初始化模块

    #region 配置服务

    /// <summary>
    /// 配置服务
    /// </summary>
    public async Task ConfigureServicesAsync()
    {
        CheckMultipleConfigureServices();

        ServiceConfigurationContext context = new(Services);
        _ = Services.AddSingleton(context);

        foreach (var module in Modules)
        {
            if (module.Instance is DddModule baseModule)
            {
                baseModule.ServiceConfigurationContext = context;
            }
        }

        // PreConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
        {
            try
            {
                await ((IPreConfigureServices)module.Instance).PreConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new InitializationException(
                    $"在模块 {module.Type.AssemblyQualifiedName} 的 {nameof(IPreConfigureServices.PreConfigureServicesAsync)} 阶段发生错误。查看集成异常以获取详细信息。",
                    ex);
            }
        }

        HashSet<Assembly> assemblies = [];

        // ConfigureServices
        foreach (var module in Modules)
        {
            if (module.Instance is DddModule { SkipAutoServiceRegistration: false })
            {
                foreach (var assembly in module.AllAssemblies)
                {
                    if (assemblies.Contains(assembly))
                    {
                        continue;
                    }

                    _ = Services.AddAssembly(assembly);
                    _ = assemblies.Add(assembly);
                }
            }

            try
            {
                await module.Instance.ConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new InitializationException(
                    $"在模块 {module.Type.AssemblyQualifiedName} 的 {nameof(IModule.ConfigureServicesAsync)} 阶段发生了一个错误。查看集成异常以获取详细信息。",
                    ex);
            }
        }

        // PostConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
        {
            try
            {
                await ((IPostConfigureServices)module.Instance).PostConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new InitializationException(
                    $"在模块 {module.Type.AssemblyQualifiedName} 的 {nameof(IPostConfigureServices.PostConfigureServicesAsync)} 阶段发生了一个错误。查看集成异常以了解详细信息。",
                    ex);
            }
        }

        foreach (var module in Modules)
        {
            if (module.Instance is DddModule baseModule)
            {
                baseModule.ServiceConfigurationContext = null!;
            }
        }

        _configuredServices = true;

        TryToSetEnvironment(Services);
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <exception cref="InitializationException"></exception>
    public void ConfigureServices()
    {
        CheckMultipleConfigureServices();

        ServiceConfigurationContext context = new(Services);
        _ = Services.AddSingleton(context);

        foreach (var module in Modules)
        {
            if (module.Instance is DddModule baseModule)
            {
                baseModule.ServiceConfigurationContext = context;
            }
        }

        // PreConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
        {
            try
            {
                ((IPreConfigureServices)module.Instance).PreConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new InitializationException(
                    $"在模块 {module.Type.AssemblyQualifiedName} 的 {nameof(IPreConfigureServices.PreConfigureServices)} 阶段发生了一个错误。查看集成异常以获取详细信息。",
                    ex);
            }
        }

        HashSet<Assembly> assemblies = [];

        // ConfigureServices
        foreach (var module in Modules)
        {
            if (module.Instance is DddModule { SkipAutoServiceRegistration: false })
            {
                foreach (var assembly in module.AllAssemblies)
                {
                    if (assemblies.Contains(assembly))
                    {
                        continue;
                    }

                    _ = Services.AddAssembly(assembly);
                    _ = assemblies.Add(assembly);
                }
            }

            try
            {
                module.Instance.ConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new InitializationException(
                    $"在模块 {module.Type.AssemblyQualifiedName} 的 {nameof(IModule.ConfigureServices)} 阶段发生了一个错误。查看集成异常以获取详细信息。",
                    ex);
            }
        }

        // PostConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
        {
            try
            {
                ((IPostConfigureServices)module.Instance).PostConfigureServices(context);
            }
            catch (Exception ex)
            {
                throw new InitializationException(
                    $"在模块 {module.Type.AssemblyQualifiedName} 的 {nameof(IPostConfigureServices.PostConfigureServices)} 阶段发生了一个错误。查看集成异常以获取详细信息。",
                    ex);
            }
        }

        foreach (var module in Modules)
        {
            if (module.Instance is DddModule baseModule)
            {
                baseModule.ServiceConfigurationContext = null!;
            }
        }

        _configuredServices = true;

        TryToSetEnvironment(Services);
    }

    /// <summary>
    /// 检查多个配置服务
    /// </summary>
    /// <exception cref="InitializationException"></exception>
    private void CheckMultipleConfigureServices()
    {
        if (_configuredServices)
        {
            throw new InitializationException(
                "服务已被配置！如果调用 ConfigureServicesAsync 方法，必须在此之前将 ApplicationCreationOptions.SkipConfigureServices 设置为 true");
        }
    }

    #endregion 配置服务

    #region 关闭应用

    /// <summary>
    /// 关闭应用
    /// </summary>
    public async Task ShutdownAsync()
    {
        using var scope = ServiceProvider.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IModuleManager>()
            .ShutdownModulesAsync(new ApplicationShutdownContext(scope.ServiceProvider));
    }

    /// <summary>
    /// 关闭应用
    /// </summary>
    public void Shutdown()
    {
        using var scope = ServiceProvider.CreateScope();
        scope.ServiceProvider.GetRequiredService<IModuleManager>()
            .ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
    }

    /// <summary>
    /// 释放
    /// </summary>
    public virtual void Dispose()
    {
        //TODO: 如果之前没有完成，就进行关闭?

        GC.SuppressFinalize(this);
    }

    #endregion 关闭应用
}
