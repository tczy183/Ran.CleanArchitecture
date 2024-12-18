using Microsoft.Extensions.Logging;

namespace Ran.Core.Ran.Exceptions;

public class BusinessException
    : Exception,
        IBusinessException,
        IHasErrorCode,
        IHasErrorDetails,
        IHasLogLevel
{
    public string? Code { get; set; }

    public string? Details { get; set; }

    public LogLevel LogLevel { get; set; }

    public BusinessException(
        string? code = null,
        string? message = null,
        string? details = null,
        Exception? innerException = null,
        LogLevel logLevel = LogLevel.Warning
    )
        : base(message, innerException)
    {
        Code = code;
        Details = details;
        LogLevel = logLevel;
    }

    public BusinessException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}
