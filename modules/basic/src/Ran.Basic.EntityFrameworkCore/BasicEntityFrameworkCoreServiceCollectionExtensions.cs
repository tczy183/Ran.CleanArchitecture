using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ran.Basic.Authorization;

namespace Ran.Basic.EntityFrameworkCore;

public static class BasicEntityFrameworkCoreServiceCollectionExtensions
{
    public static IServiceCollection AddBasicEntityFrameworkCore<TDbContext>(
        this IServiceCollection services
    )
        where TDbContext : DbContext, IBasicDbContext
    {
        services.Replace(
            ServiceDescriptor.Scoped<
                IBasicAuthorizationStore,
                EfCoreBasicAuthorizationStore<TDbContext>
            >()
        );
        return services;
    }
}
