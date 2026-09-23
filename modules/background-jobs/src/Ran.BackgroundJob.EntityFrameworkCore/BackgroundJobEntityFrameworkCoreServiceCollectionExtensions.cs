using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ran.BackgroundJob.EntityFrameworkCore;

public static class BackgroundJobEntityFrameworkCoreServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundJobEntityFrameworkCore<TDbContext>(
        this IServiceCollection services
    )
        where TDbContext : DbContext, IBackgroundJobDbContext
    {
        services.Replace(
            ServiceDescriptor.Scoped<IBackgroundJobStore, EfCoreBackgroundJobStore<TDbContext>>()
        );
        return services;
    }
}
