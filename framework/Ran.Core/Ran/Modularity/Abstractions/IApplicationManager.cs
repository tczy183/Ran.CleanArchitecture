using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IApplicationManager
{
    void ConfigureConfigureServices(IServiceCollection services);
    void Configure(
        IApplicationBuilder applicationBuilder,
        IEndpointRouteBuilder endpointRouteBuilder
    );
    void Shutdown(
        IApplicationBuilder applicationBuilder,
        IEndpointRouteBuilder endpointRouteBuilder
    );
}
