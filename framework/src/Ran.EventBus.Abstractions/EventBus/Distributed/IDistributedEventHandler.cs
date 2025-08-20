namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public interface IDistributedEventHandler<in TEvent> : IEventHandler<TEvent>
    where TEvent : class { }
