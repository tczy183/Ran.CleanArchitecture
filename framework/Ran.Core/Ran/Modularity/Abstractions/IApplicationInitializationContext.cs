using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;

namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IApplicationInitializationContext
{
    public IApplicationBuilder ApplicationBuilder { get; }

    public IEndpointRouteBuilder EndpointRouteBuilder { get; }
    public IServiceProvider ServiceProvider { get; }
}
