using System.Diagnostics.CodeAnalysis;
using Ran.Core.Utils.System;

namespace Ran.EventBus.Abstractions.EventBus.Distributed;

public class InboxConfig
{
    [NotNull]
    public string Name { get; }

    [NotNull]
    public string DatabaseName
    {
        get => _databaseName;
        set => _databaseName = CheckHelper.NotNullOrWhiteSpace(value, nameof(DatabaseName));
    }

    [NotNull]
    private string _databaseName = default!;

    public Type ImplementationType { get; set; } = default!;

    public Func<Type, bool>? EventSelector { get; set; }

    public Func<Type, bool>? HandlerSelector { get; set; }

    /// <summary>
    /// Used to enable/disable processing incoming events.
    /// Default: true.
    /// </summary>
    public bool IsProcessingEnabled { get; set; } = true;

    public InboxConfig([NotNull] string name)
    {
        Name = CheckHelper.NotNullOrWhiteSpace(name, nameof(name));
    }
}
