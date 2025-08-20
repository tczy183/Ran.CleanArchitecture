using Ran.Mediator.Requests.Send;

namespace Ran.Mediator.Requests.Notification;

public interface INotificationHandler<in TRequestEvent> : IRequestHandler
    where TRequestEvent : INotification
{
    ValueTask Handle(TRequestEvent request, CancellationToken cancellationToken);
}
