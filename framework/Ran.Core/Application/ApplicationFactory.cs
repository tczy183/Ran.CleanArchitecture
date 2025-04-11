using Ran.Core.Modularity;

namespace Ran.Core.Application;

/// <summary>
/// 应用工厂
/// </summary>
public static class ApplicationFactory
{
    #region 创建集成服务应用

    /// <summary>
    /// 创建集成服务应用，异步
    /// </summary>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static async Task<IApplicationWithInternalServiceProvider> CreateAsync<TStartupModule>(
        Action<ApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IModule
    {
        var app = await CreateAsync<TStartupModule>(options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    /// <summary>
    /// 创建集成服务应用，异步
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static async Task<IApplicationWithInternalServiceProvider> CreateAsync(
        Type startupModuleType,
        Action<ApplicationCreationOptions>? optionsAction = null)
    {
        ApplicationWithInternalServiceProvider app = new(startupModuleType, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    /// <summary>
    /// 创建集成服务应用
    /// </summary>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationWithInternalServiceProvider Create<TStartupModule>(
        Action<ApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IModule
    {
        return Create(typeof(TStartupModule), optionsAction);
    }

    /// <summary>
    /// 创建集成服务应用
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationWithInternalServiceProvider Create(
        Type startupModuleType,
        Action<ApplicationCreationOptions>? optionsAction = null)
    {
        return new ApplicationWithInternalServiceProvider(startupModuleType, optionsAction);
    }

    #endregion 创建集成服务应用

    #region 创建外部服务应用

    /// <summary>
    /// 创建外部服务应用，异步
    /// </summary>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static async Task<IApplicationWithExternalServiceProvider> CreateAsync<TStartupModule>(
        IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IModule
    {
        var app = await CreateAsync<TStartupModule>(services, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    /// <summary>
    /// 创建外部服务应用，异步
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static async Task<IApplicationWithExternalServiceProvider> CreateAsync(
        Type startupModuleType,
        IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction = null)
    {
        ApplicationWithExternalServiceProvider app = new(startupModuleType, services, options =>
        {
            options.SkipConfigureServices = true;
            optionsAction?.Invoke(options);
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    /// <summary>
    /// 创建外部服务应用
    /// </summary>
    /// <typeparam name="TStartupModule"></typeparam>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationWithExternalServiceProvider Create<TStartupModule>(
        IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }

    /// <summary>
    /// 创建外部服务应用
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IApplicationWithExternalServiceProvider Create(
        Type startupModuleType,
        IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction = null)
    {
        return new ApplicationWithExternalServiceProvider(startupModuleType, services, optionsAction);
    }

    #endregion 创建外部服务应用
}
