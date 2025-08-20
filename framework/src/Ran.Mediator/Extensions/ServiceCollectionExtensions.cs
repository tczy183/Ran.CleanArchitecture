using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ran.Mediator.Requests;

namespace Ran.Mediator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDispatchR(this IServiceCollection services)
    {
        services.TryAddScoped<IMediator, Requests.Mediator>();
        return services;
    }
}
