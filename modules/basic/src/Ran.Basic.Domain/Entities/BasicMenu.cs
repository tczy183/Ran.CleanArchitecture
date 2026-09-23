using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Basic.Entities;

public sealed class BasicMenu : AggregateRoot<Guid>
{
    private BasicMenu() { }

    public BasicMenu(Guid id, string name, string displayName, MenuType type)
    {
        Id = id;
        Name = BasicCheck.Required(name, 128, nameof(name));
        DisplayName = BasicCheck.Required(displayName, 128, nameof(displayName));
        Type = type;
    }

    public Guid? ParentId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public MenuType Type { get; private set; }

    public string? Route { get; private set; }

    public string? Component { get; private set; }

    public string? Icon { get; private set; }

    public string? PermissionName { get; private set; }

    public int Sort { get; private set; }

    public bool IsVisible { get; private set; } = true;

    public bool IsEnabled { get; private set; } = true;

    public void SetParent(Guid? parentId)
    {
        if (parentId == Id)
        {
            throw new InvalidOperationException("A menu cannot be its own parent.");
        }

        ParentId = parentId;
    }

    public void SetNavigation(string? route, string? component, string? icon)
    {
        Route = NormalizeOptional(route, 256, nameof(route));
        Component = NormalizeOptional(component, 256, nameof(component));
        Icon = NormalizeOptional(icon, 128, nameof(icon));
    }

    public void SetPermission(string? permissionName)
    {
        PermissionName = NormalizeOptional(permissionName, 256, nameof(permissionName));
    }

    public void SetDisplay(int sort, bool isVisible, bool isEnabled)
    {
        Sort = sort;
        IsVisible = isVisible;
        IsEnabled = isEnabled;
    }

    private static string? NormalizeOptional(string? value, int maxLength, string parameterName)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : BasicCheck.Required(value, maxLength, parameterName);
    }
}
