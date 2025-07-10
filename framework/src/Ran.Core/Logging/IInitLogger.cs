namespace Ran.Core.Logging;

/// <summary>
/// 初始化日志接口
/// </summary>
public interface IInitLogger<out T> : ILogger<T>
{
    /// <summary>
    /// 日志入口
    /// </summary>
    List<InitLogEntry> Entries { get; }
}
