using Ran.Core.Collections;
using Ran.EventBus.Abstractions.EventBus;

namespace Ran.EventBus.Local;

public class LocalEventBusOptions
{
    public ITypeList<IEventHandler> Handlers { get; } = new TypeList<IEventHandler>();
}
