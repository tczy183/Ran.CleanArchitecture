using Ran.Basic.Authorization;
using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Basic.Entities;

public sealed class BasicRole : AggregateRoot<Guid>
{
    private BasicRole() { }

    public BasicRole(Guid id, string name, string displayName)
    {
        Id = id;
        SetName(name, displayName);
    }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public DataScope DataScope { get; private set; }

    public bool IsStatic { get; private set; }

    public bool IsActive { get; private set; } = true;

    public void SetName(string name, string displayName)
    {
        Name = BasicCheck.Required(name, 64, nameof(name));
        DisplayName = BasicCheck.Required(displayName, 128, nameof(displayName));
    }

    public void SetDataScope(DataScope dataScope) => DataScope = dataScope;

    public void SetStatic(bool isStatic) => IsStatic = isStatic;

    public void SetActive(bool isActive) => IsActive = isActive;
}
