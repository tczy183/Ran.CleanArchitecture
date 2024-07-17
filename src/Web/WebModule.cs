using Infrastructure;

using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Web;

[DependsOn(typeof(InfrastructureModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreModule)
)]
public class WebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var host = context.Services.GetHostingEnvironment();
        var service = context.Services;
        //设置api格式
        service.AddControllers();
        service.AddSwaggerGen();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var service = context.ServiceProvider;

        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        app.UseRouting();

        if (!env.IsDevelopment())
        {
            //速率限制
            app.UseRateLimiter();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();

        app.UseConfiguredEndpoints();
        app.UseEndpoints(config =>
        {
            config.MapControllers();
        });
    }
}