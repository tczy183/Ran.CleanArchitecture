using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ran.Core.BackgroundWorkers;
using Ran.EventBus.Abstractions.EventBus.Distributed;

namespace Ran.EventBus.Distributed;

public class InboxProcessManager(
    IOptions<DistributedEventBusOptions> options,
    IServiceProvider serviceProvider
) : IBackgroundWorker
{
    protected DistributedEventBusOptions Options { get; } = options.Value;
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    protected List<IInboxProcessor> Processors { get; } = new();

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (var inboxConfig in Options.Inboxes.Values)
        {
            if (inboxConfig.IsProcessingEnabled)
            {
                var processor = ServiceProvider.GetRequiredService<IInboxProcessor>();
                await processor.StartAsync(inboxConfig, cancellationToken);
                Processors.Add(processor);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (var processor in Processors)
        {
            await processor.StopAsync(cancellationToken);
        }
    }
}
