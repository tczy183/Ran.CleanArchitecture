using Application;
using Infrastructure;
using Ran.Core.Application;
using Ran.Core.AspNetCore;
using Ran.Core.AspNetCore.Extensions;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Modularity;

namespace Web;

[DependsOn(typeof(ApplicationModule), typeof(InfrastructureModule), typeof(AspNetCoreModule))]
public class WebModule : DddModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        context.Services.AddEndpointsApiExplorer();
        context.Services.AddControllers().AddControllersAsServices();
        context.Services.AddSwaggerGen();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        // var builder = context.GetEndpointRouteBuilder();
        var env = context.GetEnvironment();

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        // app.UseAuthorization();
        // builder.MapGet("/", () => "Hello World!");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
