using Microsoft.Extensions.DependencyInjection;
using Ran.Core.BackgroundWorkers;
using Ran.Core.Ran.Modularity;
using Ran.Core.Ran.Modularity.Abstractions;
using Ran.Core.Ran.Modularity.Attributes;
using Ran.Core.Ran.Reflection;
using Ran.Core.System.Collections;
using Ran.EventBus.Abstractions;
using Ran.EventBus.Abstractions.EventBus.Distributed;
using Ran.EventBus.Abstractions.EventBus.Local;
using Ran.EventBus.Distributed;
using Ran.EventBus.Local;

namespace Ran.EventBus;

[DependsOn(typeof(EventBusAbstractionsModule))]
public class EventBusModule : BaseModule
{
    public override void PreConfigureServices(IApplicationConfigureServiceContext context)
    {
        AddEventHandlers(context.Services);
    }

    public override async Task OnApplicationInitializationAsync(
        IApplicationInitializationContext context
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
