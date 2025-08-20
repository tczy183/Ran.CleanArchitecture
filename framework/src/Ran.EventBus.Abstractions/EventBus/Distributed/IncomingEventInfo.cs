using Ran.Core.Utils.System;

namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public class IncomingEventInfo
{
    public static int MaxEventNameLength { get; set; } = 256;

    public Guid Id { get; }

    public string MessageId { get; } = default!;

    public string EventName { get; } = default!;

    public byte[] EventData { get; } = default!;

    public DateTime CreationTime { get; }

    protected IncomingEventInfo() { }

    public IncomingEventInfo(
        Guid id,
        string messageId,
        string eventName,
        byte[] eventData,
        DateTime creationTime
    )
    {
        Id = id;
        MessageId = messageId;
        EventName = CheckHelper.NotNullOrWhiteSpace(
            eventName,
            nameof(eventName),
            MaxEventNameLength
        );
        EventData = eventData;
        CreationTime = creationTime;
    }
}
