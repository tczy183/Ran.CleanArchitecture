﻿using System.Runtime.ExceptionServices;
using Ran.Core.Exceptions.Handling.Abstracts;

namespace Ran.Core.Extensions.Exceptions;

/// <summary>
/// 异常扩展方法
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// 获取异常的消息，包括内部异常
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="isHideStackTrace">是否隐藏异常规模信息</param>
    /// <returns></returns>
    public static string FormatMessage(this Exception? exception, bool isHideStackTrace = false)
    {
        if (exception is null)
        {
            return string.Empty;
        }

        var message = exception.Message;
        if (isHideStackTrace)
        {
            return message;
        }

        if (exception.InnerException is not null)
        {
            message += " --> " + exception.InnerException.FormatMessage();
        }

        return message;
    }

    /// <summary>
    /// 尝试从给定的<paramref name="exception"/>获取日志级别，如果它实现了<see cref="IHasLogLevel"/>接口
    /// 否则，返回<paramref name="defaultLevel"/>
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="defaultLevel">默认日志级别</param>
    /// <returns></returns>
    public static LogLevel GetLogLevel(
        this Exception exception,
        LogLevel defaultLevel = LogLevel.Error
    )
    {
        return (exception as IHasLogLevel)?.LogLevel ?? defaultLevel;
    }

    /// <summary>
    /// 使用<see cref="ExceptionDispatchInfo.Capture"/>方法以重新抛出异常，同时保留堆栈跟踪
    /// </summary>
    /// <param name="exception">异常将被重新抛出</param>
    public static void ReThrow(this Exception exception)
    {
        ExceptionDispatchInfo.Capture(exception).Throw();
    }

    /// <summary>
    /// 如果<paramref name="isThrow"/>为 true，则抛出异常
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="isThrow">是否抛出异常</param>
    public static void ThrowIf(this Exception exception, bool isThrow)
    {
        if (isThrow)
        {
            throw exception;
        }
    }

    /// <summary>
    /// 如果<paramref name="isThrowFunc"/>返回 true，则抛出异常
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="isThrowFunc">是否抛出异常</param>
    public static void ThrowIf(this Exception exception, Func<bool> isThrowFunc)
    {
        if (isThrowFunc())
        {
            throw exception;
        }
    }
}
