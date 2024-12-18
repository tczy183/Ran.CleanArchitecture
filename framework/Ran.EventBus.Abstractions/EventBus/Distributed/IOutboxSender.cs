namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public interface IOutboxSender
{
    Task StartAsync(OutboxConfig outboxConfig, CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);
}
