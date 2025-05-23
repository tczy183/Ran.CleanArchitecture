using Ran.Core.Utils.Collections;

namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public class InboxConfigDictionary : Dictionary<string, InboxConfig>
{
    public void Configure(Action<InboxConfig> configAction)
    {
        Configure("Default", configAction);
    }

    public void Configure(string outboxName, Action<InboxConfig> configAction)
    {
        var outboxConfig = this.GetOrAdd(outboxName, () => new InboxConfig(outboxName));
        configAction(outboxConfig);
    }
}
