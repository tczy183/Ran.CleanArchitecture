using Ran.Core.Collections;
using Ran.EventBus.Abstractions.EventBus;
using Ran.EventBus.Abstractions.EventBus.Distributed;

namespace Ran.EventBus.Distributed;

public class DistributedEventBusOptions
{
    public ITypeList<IEventHandler> Handlers { get; } = new TypeList<IEventHandler>();

    public OutboxConfigDictionary Outboxes { get; } = new();

    public InboxConfigDictionary Inboxes { get; } = new();
}
