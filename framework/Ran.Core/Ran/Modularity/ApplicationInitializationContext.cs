using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public class ApplicationInitializationContext(
    IApplicationBuilder applicationBuilder,
    IEndpointRouteBuilder endpointRouteBuilder
) : IApplicationInitializationContext
{
    public IApplicationBuilder ApplicationBuilder { get; } = applicationBuilder;
    public IEndpointRouteBuilder EndpointRouteBuilder { get; } = endpointRouteBuilder;
    public IServiceProvider ServiceProvider { get; } = applicationBuilder.ApplicationServices;
}
