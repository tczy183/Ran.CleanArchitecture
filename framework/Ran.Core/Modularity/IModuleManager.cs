using Ran.Core.Application;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块管理器接口
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// 初始化模块，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task InitializeModulesAsync(ApplicationInitializationContext context);

    /// <summary>
    /// 初始化模块
    /// </summary>
    /// <param name="context"></param>
    void InitializeModules(ApplicationInitializationContext context);

    /// <summary>
    /// 关闭模块，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ShutdownModulesAsync(ApplicationShutdownContext context);

    /// <summary>
    /// 关闭模块
    /// </summary>
    /// <param name="context"></param>
    void ShutdownModules(ApplicationShutdownContext context);
}
