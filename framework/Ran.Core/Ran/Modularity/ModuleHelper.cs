using System.Reflection;
using Microsoft.Extensions.Logging;
using Ran.Core.Ran.Modularity.Attributes;

namespace Ran.Core.Ran.Modularity;

public static class ModuleHelper
{
    public static List<Type> FindAllModuleTypes(Type startupModuleType)
    {
        var moduleTypes = new List<Type>();
        AddModules(moduleTypes, startupModuleType);
        return moduleTypes;
    }

    public static List<Type> FindDependedModuleTypes(Type moduleType)
    {
        BaseModule.CheckModuleType(moduleType);
        var source = new List<Type>();
        foreach (var dependedTypes in moduleType.GetCustomAttributes().OfType<DependsOnAttribute>())
        {
            foreach (var dependedType in dependedTypes.GetDependedTypes())
            {
                if (!source.Contains(dependedType))
                    source.Add(dependedType);
            }
        }
        return source;
    }

    private static void AddModules(List<Type> moduleTypes, Type moduleType)
    {
        BaseModule.CheckModuleType(moduleType);
        if (moduleTypes.Contains(moduleType))
            return;
        moduleTypes.Add(moduleType);
        foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
            AddModules(moduleTypes, dependedModuleType);
    }
}
