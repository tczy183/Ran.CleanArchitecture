using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ran.Core.BackgroundWorkers;
using Ran.EventBus.Abstractions.EventBus.Distributed;

namespace Ran.EventBus.Distributed;

public class OutboxSenderManager(
    IOptions<DistributedEventBusOptions> options,
    IServiceProvider serviceProvider
) : IBackgroundWorker
{
    protected DistributedEventBusOptions Options { get; } = options.Value;
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    protected List<IOutboxSender> Senders { get; } = new();

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (var outboxConfig in Options.Outboxes.Values)
        {
            if (outboxConfig.IsSendingEnabled)
            {
                var sender = ServiceProvider.GetRequiredService<IOutboxSender>();
                await sender.StartAsync(outboxConfig, cancellationToken);
                Senders.Add(sender);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (var sender in Senders)
        {
            await sender.StopAsync(cancellationToken);
        }
    }
}
