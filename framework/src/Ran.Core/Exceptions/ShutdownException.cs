using Ran.Core.Utils.Logging;

namespace Ran.Core.Exceptions;

/// <summary>
/// 关闭过程异常
/// </summary>
public class ShutdownException : Exception
{
    private const string DefaultMessage = "程序关闭过程异常。";

    /// <summary>
    /// 构造函数
    /// </summary>
    public ShutdownException()
        : base(DefaultMessage)
    {
        LogHelper.Error(DefaultMessage);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public ShutdownException(string? message)
        : base(DefaultMessage + message)
    {
        LogHelper.Error(DefaultMessage + message);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ShutdownException(string? message, Exception? innerException)
        : base(DefaultMessage + message, innerException)
    {
        LogHelper.Error(DefaultMessage + message);
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw()
    {
        throw new ShutdownException();
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw(string? message)
    {
        throw new ShutdownException(message);
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw(string? message, Exception? exception)
    {
        throw new ShutdownException(message, exception);
    }
}
