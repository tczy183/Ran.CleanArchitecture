namespace Ran.Core.Application;

/// <summary>
/// 程序关闭时接口
/// </summary>
public interface IOnApplicationShutdown
{
    /// <summary>
    /// 程序关闭时，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnApplicationShutdownAsync(ApplicationShutdownContext context);

    /// <summary>
    /// 程序关闭时
    /// </summary>
    /// <param name="context"></param>
    void OnApplicationShutdown(ApplicationShutdownContext context);
}
