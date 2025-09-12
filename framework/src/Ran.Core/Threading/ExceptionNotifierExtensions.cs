using Ran.Core.Exceptions.Handling;
using Ran.Core.Exceptions.Handling.Abstracts;
using Ran.Core.Utils.System;

namespace Ran.Core.Threading;

public static class ExceptionNotifierExtensions
{
    public static Task NotifyAsync(
        [NotNull] this IExceptionNotifier exceptionNotifier,
        [NotNull] Exception exception,
        LogLevel? logLevel = null,
        bool handled = true
    )
    {
        CheckHelper.NotNull(exceptionNotifier, nameof(exceptionNotifier));

        return exceptionNotifier.NotifyAsync(
            new ExceptionNotificationContext(exception, logLevel, handled)
        );
    }
}
