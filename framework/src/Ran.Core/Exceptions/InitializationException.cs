using Ran.Core.Utils.Logging;

namespace Ran.Core.Exceptions;

/// <summary>
/// 初始化异常
/// </summary>
public class InitializationException : Exception
{
    private const string DefaultMessage = "程序初始化异常。";

    /// <summary>
    /// 构造函数
    /// </summary>
    public InitializationException()
        : base(DefaultMessage)
    {
        LogHelper.Error(DefaultMessage);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public InitializationException(string? message)
        : base(DefaultMessage + message)
    {
        LogHelper.Error(DefaultMessage + Environment.NewLine + message);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public InitializationException(string? message, Exception? innerException)
        : base(DefaultMessage + message, innerException)
    {
        LogHelper.Error(
            DefaultMessage
                + Environment.NewLine
                + message
                + Environment.NewLine
                + innerException?.StackTrace
        );
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw()
    {
        throw new InitializationException();
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw(string? message)
    {
        throw new InitializationException(message);
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw(string? message, Exception? exception)
    {
        throw new InitializationException(message, exception);
    }
}
