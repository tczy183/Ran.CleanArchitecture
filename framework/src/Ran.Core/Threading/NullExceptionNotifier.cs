using Ran.Core.Exceptions.Handling;
using Ran.Core.Exceptions.Handling.Abstracts;

namespace Ran.Core.Threading;

public class NullExceptionNotifier : IExceptionNotifier
{
    public static NullExceptionNotifier Instance { get; } = new NullExceptionNotifier();

    private NullExceptionNotifier() { }

    public Task NotifyAsync(ExceptionNotificationContext context)
    {
        return Task.CompletedTask;
    }
}
