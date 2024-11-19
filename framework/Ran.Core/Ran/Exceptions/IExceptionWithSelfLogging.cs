using Microsoft.Extensions.Logging;

namespace Ran.Core.Ran.Exceptions;

public interface IExceptionWithSelfLogging
{
    void Log(ILogger logger);
}
