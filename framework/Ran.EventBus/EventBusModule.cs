using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Application;
using Ran.Core.BackgroundWorkers;
using Ran.Core.Extensions.DependencyInjection;
using Ran.Core.Modularity;
using Ran.Core.Utils.Collections;
using Ran.Core.Utils.Reflections;
using Ran.EventBus.Abstractions;
using Ran.EventBus.Abstractions.EventBus.Distributed;
using Ran.EventBus.Abstractions.EventBus.Local;
using Ran.EventBus.Distributed;
using Ran.EventBus.Local;

namespace Ran.EventBus;

[DependsOn(typeof(EventBusAbstractionsModule))]
public class EventBusModule : DddModule
{

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AddEventHandlers(context.Services);
    }

    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        await context.AddBackgroundWorkerAsync<OutboxSenderManager>();
        await context.AddBackgroundWorkerAsync<InboxProcessManager>();
    }

    private static void AddEventHandlers(IServiceCollection services)
    {
        var localHandlers = new List<Type>();
        var distributedHandlers = new List<Type>();

        services.OnRegistered(context =>
        {
            if (
                ReflectionHelper.IsAssignableToGenericType(
                    context.ImplementationType,
                    typeof(ILocalEventHandler<>)
                )
            )
            {
                localHandlers.Add(context.ImplementationType);
            }

            if (
                ReflectionHelper.IsAssignableToGenericType(
                    context.ImplementationType,
                    typeof(IDistributedEventHandler<>)
                )
            )
            {
                distributedHandlers.Add(context.ImplementationType);
            }
        });
        services.Configure<LocalEventBusOptions>(options =>
        {
            options.Handlers.AddIfNotContains(localHandlers);
        });

        services.Configure<DistributedEventBusOptions>(options =>
        {
            options.Handlers.AddIfNotContains(distributedHandlers);
        });
    }
}
