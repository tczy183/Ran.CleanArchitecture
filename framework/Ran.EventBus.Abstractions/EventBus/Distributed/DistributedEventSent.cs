namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public class DistributedEventSent
{
    public DistributedEventSource Source { get; set; }

    public string EventName { get; set; } = default!;

    public object EventData { get; set; } = default!;
}
