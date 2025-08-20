using Ran.Core.Utils.System;

namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public class OutgoingEventInfo
{
    public static int MaxEventNameLength { get; set; } = 256;

    public Guid Id { get; }

    public string EventName { get; } = default!;

    public byte[] EventData { get; } = default!;

    public DateTime CreationTime { get; }

    protected OutgoingEventInfo() { }

    public OutgoingEventInfo(Guid id, string eventName, byte[] eventData, DateTime creationTime)
    {
        Id = id;
        EventName = CheckHelper.NotNullOrWhiteSpace(
            eventName,
            nameof(eventName),
            MaxEventNameLength
        );
        EventData = eventData;
        CreationTime = creationTime;
    }
}
