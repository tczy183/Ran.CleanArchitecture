using Microsoft.Extensions.DependencyInjection;
using Ran.Core.Ran.Modularity.Abstractions;

namespace Ran.Core.Ran.Modularity;

public class ModuleLoader : IModuleLoader
{
    public IModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType)
    {
        var moduleDescriptors = CreateModuleDescriptors(services, startupModuleType);

        var startupDependedModule = moduleDescriptors.FirstOrDefault(m =>
            m.ModuleType == startupModuleType
        );
        return SortByModuleDescriptor(startupDependedModule!).Cast<IModuleDescriptor>().ToArray();
    }

    private List<ModuleDescriptor> CreateModuleDescriptors(
        IServiceCollection services,
        Type startupModuleType
    )
    {
        var moduleDescriptors = new List<ModuleDescriptor>();
        var allModules = ModuleHelper.FindAllModuleTypes(startupModuleType);
        var serviceProvider = services.BuildServiceProvider();
        foreach (var moduleType in allModules)
        {
            var instance = (IModule)ActivatorUtilities.CreateInstance(serviceProvider, moduleType);
            moduleDescriptors.Add(new ModuleDescriptor(moduleType, instance));
        }

        foreach (var moduleDescriptor in moduleDescriptors)
        {
            moduleDescriptor.SetDependencies(moduleDescriptors);
        }

        return moduleDescriptors;
    }

    private List<IModuleDescriptor> SortByModuleDescriptor(ModuleDescriptor moduleDescriptor)
    {
        var sorted = new List<IModuleDescriptor>();
        var visited = new Dictionary<IModuleDescriptor, bool>();
        SortByDependenciesVisit(moduleDescriptor, m => m.Dependencies, sorted, visited);
        return sorted;
    }

    private void SortByDependenciesVisit(
        IModuleDescriptor item,
        Func<IModuleDescriptor, IEnumerable<IModuleDescriptor>> getDependencies,
        List<IModuleDescriptor> sorted,
        Dictionary<IModuleDescriptor, bool> visited
    )
    {
        var alreadyVisited = visited.TryGetValue(item, out var inProcess);
        if (alreadyVisited)
        {
            if (inProcess)
            {
                throw new ArgumentException("Cyclic dependency found! Item: " + item);
            }
        }
        else
        {
            visited[item] = true;

            var dependencies = getDependencies(item);
            foreach (var dependency in dependencies)
            {
                SortByDependenciesVisit(dependency, getDependencies, sorted, visited);
            }

            visited[item] = false;
            sorted.Add(item);
        }
    }
}
