using Ran.Mediator.Requests.Notification;

namespace Application.Users;

public record UserCreateNotification(string Name) : INotification;
