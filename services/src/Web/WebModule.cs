using Application;
using Infrastructure;
using Ran.Core.Application;
using Ran.Core.AspNetCore.Extensions;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Modularity;
using Ran.EventBus;

namespace Web;

[DependsOn(
    typeof(ApplicationModule),
    typeof(InfrastructureModule)
    )]
public class WebModule : BaseModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        context.Services.AddEndpointsApiExplorer();
        context.Services.AddControllers().AddControllersAsServices();
        context.Services.AddSwaggerGen();
        context.Services.AddObjectAccessor<IApplicationBuilder>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
