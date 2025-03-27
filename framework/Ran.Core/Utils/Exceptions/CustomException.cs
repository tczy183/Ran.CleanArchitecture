using Ran.Core.Utils.Logging;

namespace Ran.Core.Utils.Exceptions;

/// <summary>
/// 自定义异常
/// </summary>
public class CustomException : Exception
{
    private const string DefaultMessage = "自定义异常。";

    /// <summary>
    /// 构造函数
    /// </summary>
    public CustomException() : base(DefaultMessage)
    {
        LogHelper.Error(DefaultMessage);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public CustomException(string? message) : base(DefaultMessage + message)
    {
        LogHelper.Error(DefaultMessage + message);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public CustomException(string? message, Exception? exception) : base(DefaultMessage + message, exception)
    {
        LogHelper.Error(DefaultMessage + message);
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw()
    {
        throw new CustomException();
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw(string? message)
    {
        throw new CustomException(message);
    }

    /// <summary>
    /// 抛出异常
    /// </summary>
    public static void Throw(string? message, Exception? exception)
    {
        throw new CustomException(message, exception);
    }
}
