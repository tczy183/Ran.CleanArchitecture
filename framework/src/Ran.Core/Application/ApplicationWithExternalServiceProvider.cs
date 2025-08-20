using Ran.Core.Utils.System;

namespace Ran.Core.Application;

/// <summary>
/// 具有外部服务的应用提供器
/// </summary>
internal sealed class ApplicationWithExternalServiceProvider
    : ApplicationBase,
        IApplicationWithExternalServiceProvider
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    public ApplicationWithExternalServiceProvider(
        Type startupModuleType,
        IServiceCollection services,
        Action<ApplicationCreationOptions>? optionsAction
    )
        : base(startupModuleType, services, optionsAction)
    {
        _ = services.AddSingleton<IApplicationWithExternalServiceProvider>(this);
    }

    /// <summary>
    /// 设置服务提供器，但不初始化模块
    /// </summary>
    void IApplicationWithExternalServiceProvider.SetServiceProvider(
        IServiceProvider serviceProvider
    )
    {
        _ = CheckHelper.NotNull(serviceProvider, nameof(serviceProvider));

        if (ServiceProvider is not null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new Exception("服务提供器之前已设置为另一个服务提供器实例！");
            }

            return;
        }

        SetServiceProvider(serviceProvider);
    }

    /// <summary>
    /// 设置服务提供器并初始化所有模块，异步
    /// 如果之前调用过 <see cref="IApplicationWithExternalServiceProvider.SetServiceProvider"/>，则应将相同的 <paramref name="serviceProvider"/> 实例传递给此方法
    /// </summary>
    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        _ = CheckHelper.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        await InitializeModulesAsync();
    }

    /// <summary>
    /// 设置服务提供器并初始化所有模块，异步
    /// 如果之前调用过 <see cref="IApplicationWithExternalServiceProvider.SetServiceProvider"/>，则应将相同的 <paramref name="serviceProvider"/> 实例传递给此方法
    /// </summary>
    public void Initialize(IServiceProvider serviceProvider)
    {
        _ = CheckHelper.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        InitializeModules();
    }

    /// <summary>
    /// 释放
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }
}
