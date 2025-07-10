namespace Ran.Core.Application;

/// <summary>
/// 程序初始化前接口
/// </summary>
public interface IOnPreApplicationInitialization
{
    /// <summary>
    /// 程序初始化前，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context);

    /// <summary>
    /// 程序初始化前
    /// </summary>
    /// <param name="context"></param>
    void OnPreApplicationInitialization(ApplicationInitializationContext context);
}
