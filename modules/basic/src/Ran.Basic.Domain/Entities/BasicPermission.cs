using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Basic.Entities;

public sealed class BasicPermission : AggregateRoot<Guid>
{
    private BasicPermission() { }

    public BasicPermission(Guid id, string name, string displayName)
    {
        Id = id;
        Name = BasicCheck.Required(name, 256, nameof(name));
        DisplayName = BasicCheck.Required(displayName, 256, nameof(displayName));
    }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public string? ParentName { get; private set; }

    public bool IsEnabled { get; private set; } = true;

    public void SetParent(string? parentName)
    {
        ParentName = string.IsNullOrWhiteSpace(parentName)
            ? null
            : BasicCheck.Required(parentName, 256, nameof(parentName));
    }

    public void SetEnabled(bool isEnabled) => IsEnabled = isEnabled;
}
