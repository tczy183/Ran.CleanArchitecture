namespace Ran.EventBus.Abstractions.EventBus;

/// <summary>
/// Indirect base interface for all event handlers.
/// </summary>
public interface IEventHandler
{
}

public interface IEventHandler<in TEvent> : IEventHandler
{
    /// <summary>
    /// Handler handles the event by implementing this method.
    /// </summary>
    /// <param name="eventData">Event data</param>
    Task HandleEventAsync(TEvent eventData);
}
