namespace Ran.Core.Ran.Modularity.Abstractions;

public interface IModuleDescriptor
{
    Type ModuleType { get; }

    IModule Instance { get; }

    IReadOnlyList<IModuleDescriptor> Dependencies { get; }
}
