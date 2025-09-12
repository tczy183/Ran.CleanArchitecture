using Ran.Core.DependencyInjection.ServiceLifetimes;
using Ran.Mediator.Requests.Send;

namespace Application.Users;

public record UserRequest(string Name) : IRequest<UserRequest, string>;
