using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public static class ApplicationBuilderExtensions
{
    //新增endpointRouteBuilder参数,兼容net8以后的MinimalAPI
    public static void BuildApplicationBuilder(
        this IApplicationBuilder applicationBuilder,
        IEndpointRouteBuilder endpointRouteBuilder
    )
    {
        var application =
            applicationBuilder.ApplicationServices.GetRequiredService<IApplicationManager>();

        application.Configure(applicationBuilder, endpointRouteBuilder);

        var requiredService =
            applicationBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        requiredService.ApplicationStopping.Register(
            () => application.Shutdown(applicationBuilder, endpointRouteBuilder)
        );
    }
}
