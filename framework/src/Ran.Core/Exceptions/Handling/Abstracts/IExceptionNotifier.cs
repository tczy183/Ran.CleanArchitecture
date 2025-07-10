namespace Ran.Core.Exceptions.Handling.Abstracts;

/// <summary>
/// 异常通知器
/// </summary>
public interface IExceptionNotifier
{
    /// <summary>
    /// 通知
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task NotifyAsync(ExceptionNotificationContext context);
}
