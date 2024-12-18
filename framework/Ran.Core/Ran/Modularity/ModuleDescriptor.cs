using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public class ModuleDescriptor : IModuleDescriptor
{
    public Type ModuleType { get; }

    public IModule Instance { get; }

    public IReadOnlyList<IModuleDescriptor> Dependencies => _dependencies;

    private readonly List<IModuleDescriptor> _dependencies;

    public ModuleDescriptor(Type moduleType, IModule instance)
    {
        ModuleType = moduleType ?? throw new ArgumentNullException(nameof(moduleType));
        Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        _dependencies = new List<IModuleDescriptor>();
    }

    public void SetDependencies(List<ModuleDescriptor> modules)
    {
        foreach (var dependedModuleType in ModuleHelper.FindDependedModuleTypes(ModuleType))
        {
            var dependedModule = modules.FirstOrDefault(m => m.ModuleType == dependedModuleType);
            if (dependedModule == null)
            {
                throw new Exception(
                    "Could not find a depended module "
                        + dependedModuleType.AssemblyQualifiedName
                        + " for "
                        + ModuleType.AssemblyQualifiedName
                );
            }

            _dependencies.Add(dependedModule);
        }
    }
}
