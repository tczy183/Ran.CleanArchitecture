using Ran.Core.Application;
using Ran.Core.Modularity;
using IHostEnvironment = Ran.Core.Application.IHostEnvironment;

namespace Ran.Core.Extensions.DependencyInjection;

/// <summary>
/// 服务容器应用程序扩展
/// </summary>
public static class ServiceCollectionApplicationExtensions
{
    /// <summary>
    /// 添加应用程序
    /// </summary>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationWithExternalServiceProvider AddApplication<TStartupModule>(
        this IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IModule
    {
        return ApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    /// <summary>
    /// 添加应用程序
    /// </summary>
    /// <param name="services"></param>
    /// <param name="startupModuleType"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationWithExternalServiceProvider AddApplication(
        this IServiceCollection services,
        Type startupModuleType,
        Action<ApplicationCreationOptions>? optionsAction = null
    )
    {
        return ApplicationFactory.Create(startupModuleType, services, optionsAction);
    }

    /// <summary>
    /// 添加应用程序
    /// </summary>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static async Task<IApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
        this IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IModule
    {
        return await ApplicationFactory.CreateAsync<TStartupModule>(services, optionsAction);
    }

    /// <summary>
    /// 添加应用程序
    /// </summary>
    /// <param name="services"></param>
    /// <param name="startupModuleType"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static async Task<IApplicationWithExternalServiceProvider> AddApplicationAsync(
        this IServiceCollection services,
        Type startupModuleType,
        Action<ApplicationCreationOptions>? optionsAction = null
    )
    {
        return await ApplicationFactory.CreateAsync(startupModuleType, services, optionsAction);
    }

    /// <summary>
    /// 获取应用程序名称
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static string? GetApplicationName(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfoAccessor>().ApplicationName;
    }

    /// <summary>
    /// 获取应用程序实例 Id
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static string GetApplicationInstanceId(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfoAccessor>().InstanceId;
    }

    /// <summary>
    /// 获取应用程序环境
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IHostEnvironment GetXiHanHostEnvironment(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IHostEnvironment>();
    }
}
