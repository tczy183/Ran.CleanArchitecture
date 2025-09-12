namespace Ran.Core.Exceptions;

/// <summary>
/// 用户友好异常
/// </summary>
public class UserFriendlyException : BusinessException, IUserFriendlyException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    /// <param name="details"></param>
    /// <param name="innerException"></param>
    /// <param name="logLevel"></param>
    public UserFriendlyException(
        string message,
        string? code = null,
        string? details = null,
        Exception? innerException = null,
        LogLevel logLevel = LogLevel.Warning
    )
        : base(code, message, details, innerException, logLevel)
    {
        Details = details;
    }
}
