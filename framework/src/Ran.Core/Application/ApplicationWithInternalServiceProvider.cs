using Ran.Core.Extensions.DependencyInjection;

namespace Ran.Core.Application;

/// <summary>
/// 具有集成服务的应用提供器
/// </summary>
public class ApplicationWithInternalServiceProvider : ApplicationBase, IApplicationWithInternalServiceProvider
{
    /// <summary>
    /// 作用域服务
    /// </summary>
    public IServiceScope? ServiceScope { get; private set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="optionsAction"></param>
    public ApplicationWithInternalServiceProvider(Type startupModuleType,
        Action<ApplicationCreationOptions>? optionsAction)
        : this(startupModuleType, new ServiceCollection(), optionsAction)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    private ApplicationWithInternalServiceProvider(
        Type startupModuleType,
        IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction) : base(startupModuleType, services, optionsAction)
    {
        _ = Services.AddSingleton<IApplicationWithInternalServiceProvider>(this);
    }

    /// <summary>
    /// 创建服务提供器，但不初始化模块。
    /// 多次调用将返回相同的服务提供器，而不会再次创建
    /// </summary>
    public IServiceProvider CreateServiceProvider()
    {
        if (ServiceProvider is not null)
        {
            return ServiceProvider;
        }

        ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        SetServiceProvider(ServiceScope.ServiceProvider);

        return ServiceProvider!;
    }

    /// <summary>
    /// 创建服务提供商并初始化所有模块，异步
    /// 如果之前调用过 <see cref="CreateServiceProvider"/> 方法，它不会重新创建，而是使用之前的那个
    /// </summary>
    public async Task InitializeAsync()
    {
        _ = CreateServiceProvider();
        await InitializeModulesAsync();
    }

    /// <summary>
    /// 创建服务提供商并初始化所有模块，异步
    /// 如果之前调用过 <see cref="CreateServiceProvider"/> 方法，它不会重新创建，而是使用之前的那个
    /// </summary>
    public void Initialize()
    {
        _ = CreateServiceProvider();
        InitializeModules();
    }

    /// <summary>
    /// 释放
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
        ServiceScope?.Dispose();

        GC.SuppressFinalize(this);
    }
}
