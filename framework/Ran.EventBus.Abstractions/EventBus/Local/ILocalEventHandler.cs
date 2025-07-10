namespace Ran.EventBus.Abstractions.EventBus.Local;

public interface ILocalEventHandler<in TEvent> : IEventHandler<TEvent>
    where TEvent : class
{ }
