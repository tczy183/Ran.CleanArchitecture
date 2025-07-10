using Ran.Core.Extensions.Exceptions;
using Ran.Core.Utils.System;

namespace Ran.Core.Exceptions.Handling;

/// <summary>
/// 异常通知上下文
/// </summary>
public class ExceptionNotificationContext
{
    /// <summary>
    /// 异常
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel LogLevel { get; }

    /// <summary>
    /// 是否已处理
    /// </summary>
    public bool Handled { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="logLevel"></param>
    /// <param name="handled"></param>
    public ExceptionNotificationContext(Exception exception, LogLevel? logLevel = null, bool handled = true)
    {
        Exception = CheckHelper.NotNull(exception, nameof(exception));
        LogLevel = logLevel ?? exception.GetLogLevel();
        Handled = handled;
    }
}
