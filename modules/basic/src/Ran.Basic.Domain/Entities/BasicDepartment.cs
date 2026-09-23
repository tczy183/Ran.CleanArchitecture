using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Basic.Entities;

public sealed class BasicDepartment : AggregateRoot<Guid>
{
    private BasicDepartment() { }

    public BasicDepartment(Guid id, string name, string displayName)
    {
        Id = id;
        Name = BasicCheck.Required(name, 64, nameof(name));
        DisplayName = BasicCheck.Required(displayName, 128, nameof(displayName));
    }

    public Guid? ParentId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public int Sort { get; private set; }

    public bool IsActive { get; private set; } = true;

    public void SetParent(Guid? parentId)
    {
        if (parentId == Id)
        {
            throw new InvalidOperationException("A department cannot be its own parent.");
        }

        ParentId = parentId;
    }

    public void SetDisplay(int sort, bool isActive)
    {
        Sort = sort;
        IsActive = isActive;
    }
}
