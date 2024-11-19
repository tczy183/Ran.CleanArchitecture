namespace Ran.Core.Ran.Modularity.Attributes;

/// <summary>
/// Used to define dependencies of a type.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute(params Type[]? dependedTypes) : Attribute, IDependedTypesProvider
{
    private Type[] DependedTypes { get; } = dependedTypes ?? Type.EmptyTypes;

    public virtual Type[] GetDependedTypes()
    {
        return DependedTypes;
    }
}
