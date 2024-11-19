namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public interface IEventOutbox
{
    Task EnqueueAsync(OutgoingEventInfo outgoingEvent);

    Task<List<OutgoingEventInfo>> GetWaitingEventsAsync(
        int maxCount,
        CancellationToken cancellationToken = default
    );

    Task DeleteAsync(Guid id);

    Task DeleteManyAsync(IEnumerable<Guid> ids);
}
