using Microsoft.Extensions.Logging;
using Ran.Mediator.Requests.Notification;

namespace Application.Users;

public class UserCreateNotificationHandler(ILogger<UserCreateNotificationHandler> logger): INotificationHandler<UserCreateNotification>
{
    public async ValueTask Handle(UserCreateNotification request, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        logger.LogInformation("User created: {RequestName}", request.Name);
    }
}
