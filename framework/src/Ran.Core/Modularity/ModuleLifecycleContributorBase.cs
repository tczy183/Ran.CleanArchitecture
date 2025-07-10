using Ran.Core.Application;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块生命周期贡献者基类
/// </summary>
public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    public virtual Task InitializeAsync(ApplicationInitializationContext context, IModule module)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    public virtual void Initialize(ApplicationInitializationContext context, IModule module)
    {
    }

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    public virtual Task ShutdownAsync(ApplicationShutdownContext context, IModule module)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="context"></param>
    /// <param name="module"></param>
    public virtual void Shutdown(ApplicationShutdownContext context, IModule module)
    {
    }
}
