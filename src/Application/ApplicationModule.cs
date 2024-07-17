using System.Reflection;

using Application.Common.Behaviours;

using Domain;

using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;

namespace Application;

[DependsOn(typeof(DomainModule))]
public class ApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        context.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        context.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
    }
}