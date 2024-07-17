using Application;
using Application.Common.Interfaces;

using Infrastructure.EntityFrameworkCore;
using Infrastructure.EntityFrameworkCore.Interceptors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace Infrastructure;

[DependsOn(
    typeof(ApplicationModule)
)]
public class InfrastructureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var masterDBCon = configuration.GetConnectionString("MasterDbConnection");
        var slaveDBCon = configuration.GetConnectionString("SlaveDbConnection");

        context.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        context.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        context.Services.AddDbContext<ApplicationWriteDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseMySql(masterDBCon, ServerVersion.AutoDetect(masterDBCon));
        });

        context.Services.AddDbContext<ApplicationReadDbContext>((sp, options) =>
        {
            options.UseMySql(slaveDBCon, ServerVersion.AutoDetect(slaveDBCon));
        });

        context.Services.AddScoped<IApplicationWriteDbContext>(provider =>
            provider.GetRequiredService<ApplicationWriteDbContext>());
    }
}