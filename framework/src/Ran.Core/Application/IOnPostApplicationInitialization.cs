namespace Ran.Core.Application;

/// <summary>
/// 程序初始化后接口
/// </summary>
public interface IOnPostApplicationInitialization
{
    /// <summary>
    /// 程序初始化后，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context);

    /// <summary>
    /// 程序初始化后
    /// </summary>
    /// <param name="context"></param>
    void OnPostApplicationInitialization(ApplicationInitializationContext context);
}
