using Application;
using Application.Users;
using Infrastructure;
using Ran.Core.Application;
using Ran.Core.AspNetCore;
using Ran.Core.AspNetCore.Extensions;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Modularity;
using Ran.Mediator;
using Ran.Mediator.Requests;

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
        var builder = context.GetEndpointRouteBuilder();
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
        builder.MapGet("/Name", (IMediator mediator ,CancellationToken cancellationToken) =>
        {
            var valueTask = mediator.Send(new UserRequest("张三"),cancellationToken);
            return valueTask;
        });

        builder.MapGet("/Name1", (IMediator mediator ,CancellationToken cancellationToken) =>
        {
            var valueTask = mediator.Publish(new UserCreateNotification("张三"),cancellationToken);
            return valueTask;
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
