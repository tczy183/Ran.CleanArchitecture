
using Ran.Core.Utils.Collections;

namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public class OutboxConfigDictionary : Dictionary<string, OutboxConfig>
{
    public void Configure(Action<OutboxConfig> configAction)
    {
        Configure("Default", configAction);
    }

    public void Configure(string outboxName, Action<OutboxConfig> configAction)
    {
        var outboxConfig = this.GetOrAdd(outboxName, () => new OutboxConfig(outboxName));
        configAction(outboxConfig);
    }
}
