using Ran.Core.DependencyInjection;
using Ran.Core.DependencyInjection.ServiceLifetimes;
using Ran.Mediator.Requests.Send;

namespace Application.Users;

public class UserRequestHandler : IRequestHandler<UserRequest, string>
{
    public string Handle(UserRequest request, CancellationToken cancellationToken)
    {
        return "User" + request.Name;
    }
}
