using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ran.Core.Ran.DependencyInjection;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public static class ApplicationConfigureServiceContextExtensions
{
    public static IApplicationBuilder GetApplicationBuilder(
        this IApplicationInitializationContext context
    )
    {
        return context.ApplicationBuilder;
    }

    public static IWebHostEnvironment GetEnvironment(this IApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    }

    public static IWebHostEnvironment? GetEnvironmentOrNull(
        this IApplicationInitializationContext context
    )
    {
        return context.ServiceProvider.GetService<IWebHostEnvironment>();
    }

    public static IConfiguration GetConfiguration(this IApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    public static ILoggerFactory GetLoggerFactory(this IApplicationInitializationContext context)
    {
        return context.ServiceProvider.GetRequiredService<ILoggerFactory>();
    }

    public static IEndpointRouteBuilder GetEndpointRouteBuilder(
        this IApplicationInitializationContext context
    )
    {
        return context.EndpointRouteBuilder;
    }
}
