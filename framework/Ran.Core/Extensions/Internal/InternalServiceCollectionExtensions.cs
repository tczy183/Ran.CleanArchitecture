using Ran.Core.Application;
using Ran.Core.DependencyInjection;
using Ran.Core.Extensions.Configuration;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Logging;
using Ran.Core.Modularity;
using Ran.Core.Reflection;
using Ran.Core.SimpleStateChecking;

namespace Ran.Core.Extensions.Internal;

/// <summary>
/// 集成服务集合扩展方法
/// </summary>
internal static class InternalServiceCollectionExtensions
{
    /// <summary>
    /// 添加核心服务
    /// </summary>
    /// <param name="services"></param>
    internal static void AddCoreServices(this IServiceCollection services)
    {
        _ = services.AddOptions();
        _ = services.AddLogging();
        _ = services.AddLocalization();
    }

    /// <summary>
    /// 添加核心服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="application"></param>
    /// <param name="applicationCreationOptions"></param>
    internal static void AddCoreServices(this IServiceCollection services, IApplication application,
        ApplicationCreationOptions applicationCreationOptions)
    {
        var moduleLoader = new ModuleLoader();
        var assemblyFinder = new AssemblyFinder(application);
        var typeFinder = new TypeFinder(assemblyFinder);

        if (!services.IsAdded<IConfiguration>())
        {
            _ = services.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(applicationCreationOptions.Configuration));
        }

        services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
        services.TryAddSingleton<ITypeFinder>(typeFinder);
        services.TryAddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory());
        services.TryAddSingleton<IModuleLoader>(moduleLoader);

        // 属性或字段自动注入服务
        _ = services.AddSingleton<AutowiredServiceHandler>();

        _ = services.AddAssemblyOf<IApplication>();

        _ = services.AddTransient(typeof(ISimpleStateCheckerManager<>), typeof(SimpleStateCheckerManager<>));

        _ = services.Configure<ModuleLifecycleOptions>(options =>
        {
            options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
        });
    }
}
