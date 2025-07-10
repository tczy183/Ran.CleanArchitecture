namespace Ran.Core.Exceptions.Handling.Abstracts;

/// <summary>
/// 日志级别接口
/// </summary>
public interface IHasLogLevel
{
    /// <summary>
    /// 日志级别
    /// </summary>
    LogLevel LogLevel { get; set; }
}
