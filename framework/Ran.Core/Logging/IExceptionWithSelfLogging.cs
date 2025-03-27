namespace Ran.Core.Logging;

/// <summary>
/// 自身日志接口
/// </summary>
public interface IExceptionWithSelfLogging
{
    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="logger"></param>
    void Log(ILogger logger);
}
