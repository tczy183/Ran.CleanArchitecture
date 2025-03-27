namespace Ran.Core.Logging;

/// <summary>
/// 初始化日志入口
/// </summary>
public class InitLogEntry
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public LogLevel LogLevel { get; init; }

    /// <summary>
    /// 事件标识符
    /// </summary>
    public EventId EventId { get; init; }

    /// <summary>
    /// 状态
    /// </summary>
    public object State { get; init; } = default!;

    /// <summary>
    /// 异常
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// 格式化器
    /// </summary>
    public Func<object, Exception?, string> Formatter { get; init; } = default!;

    /// <summary>
    /// 异常消息
    /// </summary>
    public string Message => Formatter(State, Exception);
}
