namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public interface IDistributedEventBus : IEventBus
{
    Task PublishAsync<TEvent>(
        TEvent eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true
    )
        where TEvent : class;

    Task PublishAsync(
        Type eventType,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true
    );
}
