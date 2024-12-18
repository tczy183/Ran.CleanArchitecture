using Microsoft.Extensions.Logging;

namespace Ran.Core.Ran.Exceptions;

public interface IHasLogLevel
{
    /// <summary>
    /// Log severity.
    /// </summary>
    LogLevel LogLevel { get; set; }
}
