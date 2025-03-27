namespace Ran.Core.Exceptions.Handling.Abstracts;

/// <summary>
/// 异常订阅者接口
/// </summary>
public interface IExceptionSubscriber
{
    /// <summary>
    /// 处理异常，异步
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task HandleAsync(ExceptionNotificationContext context);
}
