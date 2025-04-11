using System.Diagnostics.CodeAnalysis;
using Ran.Core.Utils.System;

namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public class OutboxConfig
{
    public string Name { get; }

    public string DatabaseName { get; set; } = default!;

    public Type ImplementationType { get; set; } = default!;

    public Func<Type, bool>? Selector { get; set; }

    /// <summary>
    /// Used to enable/disable sending events from outbox to the message broker.
    /// Default: true.
    /// </summary>
    public bool IsSendingEnabled { get; set; } = true;

    public OutboxConfig([NotNull] string name)
    {
        Name = CheckHelper.NotNullOrWhiteSpace(name, nameof(name));
    }
}
