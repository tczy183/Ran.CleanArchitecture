using Ran.Core.Application;
using Ran.Core.DependencyInjection.ServiceLifetimes;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块生命周期贡献者接口
/// </summary>
public interface IModuleLifecycleContributor : ITransientDependency
{
    /// <summary>
    /// 初始化，异步
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    Task InitializeAsync(ApplicationInitializationContext context, IModule module);

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    void Initialize(ApplicationInitializationContext context, IModule module);

    /// <summary>
    /// 关闭，异步
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    Task ShutdownAsync(ApplicationShutdownContext context, IModule module);

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    void Shutdown(ApplicationShutdownContext context, IModule module);
}
