﻿using Ran.Core.Exceptions.Handling.Abstracts;
using Ran.Core.Extensions.Exceptions;
using Ran.Core.Logging;
using Ran.Core.Utils.Collections;

namespace Ran.Core.Extensions.Logging;

/// <summary>
/// 日志扩展方法
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    public static void LogWithLevel(this ILogger logger, LogLevel logLevel, string message)
    {
        switch (logLevel)
        {
            case LogLevel.Critical:
                logger.LogCritical("{Message}", message);
                break;

            case LogLevel.Error:
                logger.LogError("{Message}", message);
                break;

            case LogLevel.Warning:
                logger.LogWarning("{Message}", message);
                break;

            case LogLevel.Information:
                logger.LogInformation("{Message}", message);
                break;

            case LogLevel.Trace:
                logger.LogTrace("{Message}", message);
                break;

            // LogLevel.Debug || LogLevel.None
            // case LogLevel.Debug:
            // case LogLevel.None:
            default:
                logger.LogDebug("{Message}", message);
                break;
        }
    }

    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public static void LogWithLevel(this ILogger logger, LogLevel logLevel, string message, Exception exception)
    {
        switch (logLevel)
        {
            case LogLevel.Critical:
                logger.LogCritical("{Exception}{Message}", exception, message);
                break;

            case LogLevel.Error:
                logger.LogError("{Exception}{Message}", exception, message);
                break;

            case LogLevel.Warning:
                logger.LogWarning("{Exception}{Message}", exception, message);
                break;

            case LogLevel.Information:
                logger.LogInformation("{Exception}{Message}", exception, message);
                break;

            case LogLevel.Trace:
                logger.LogTrace("{Exception}{Message}", exception, message);
                break;

            // LogLevel.Debug || LogLevel.None
            // case LogLevel.Debug:
            // case LogLevel.None:
            default:
                logger.LogDebug("{Exception}{Message}", exception, message);
                break;
        }
    }

    /// <summary>
    /// 记录异常
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="ex"></param>
    /// <param name="level"></param>
    public static void LogException(this ILogger logger, Exception ex, LogLevel? level = null)
    {
        var selectedLevel = level ?? ex.GetLogLevel();

        logger.LogWithLevel(selectedLevel, ex.Message, ex);
        LogKnownProperties(logger, ex, selectedLevel);
        LogSelfLogging(logger, ex);
        LogData(logger, ex, selectedLevel);
    }

    /// <summary>
    /// 记录已知异常属性
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="logLevel"></param>
    private static void LogKnownProperties(ILogger logger, Exception exception, LogLevel logLevel)
    {
        if (exception is IHasErrorCode exceptionWithErrorCode)
        {
            logger.LogWithLevel(logLevel, "异常代码:" + exceptionWithErrorCode.Code);
        }

        if (exception is IHasErrorDetails exceptionWithErrorDetails)
        {
            logger.LogWithLevel(logLevel, "异常详情:" + exceptionWithErrorDetails.Details);
        }
    }

    /// <summary>
    /// 记录异常数据
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="logLevel"></param>
    private static void LogData(ILogger logger, Exception exception, LogLevel logLevel)
    {
        if (exception.Data.Count <= 0)
        {
            return;
        }

        StringBuilder exceptionData = new();
        _ = exceptionData.AppendLine("---------- 异常数据 ----------");
        foreach (var key in exception.Data.Keys)
        {
            _ = exceptionData.AppendLine($"{key} = {exception.Data[key]}");
        }

        logger.LogWithLevel(logLevel, exceptionData.ToString());
    }

    /// <summary>
    /// 记录自身日志
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    private static void LogSelfLogging(ILogger logger, Exception exception)
    {
        List<IExceptionWithSelfLogging> loggingExceptions = [];

        if (exception is IExceptionWithSelfLogging logging)
        {
            loggingExceptions.Add(logging);
        }
        else if (exception is AggregateException { InnerException: not null } aggException)
        {
            if (aggException.InnerException is IExceptionWithSelfLogging selfLogging)
            {
                loggingExceptions.Add(selfLogging);
            }

            foreach (var innerException in aggException.InnerExceptions)
            {
                if (innerException is IExceptionWithSelfLogging withSelfLogging)
                {
                    _ = loggingExceptions.AddIfNotContains(withSelfLogging);
                }
            }
        }

        foreach (var ex in loggingExceptions)
        {
            ex.Log(logger);
        }
    }
}
