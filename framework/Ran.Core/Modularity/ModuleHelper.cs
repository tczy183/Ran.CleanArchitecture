using Ran.Core.Utils.Collections;
using Ran.Core.Utils.Logging;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块帮助类
/// </summary>
public static class ModuleHelper
{
    /// <summary>
    /// 查找所有模块类型
    /// </summary>
    /// <param name="startupModuleType"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static List<Type> FindAllModuleTypes(Type startupModuleType, ILogger? logger)
    {
        List<Type> moduleTypes = [];
        logger?.LogInformation("加载模块:");
        AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType, logger);
        logger?.LogInformation("已初始化所有模块。");
        return moduleTypes;
    }

    /// <summary>
    /// 查找依赖模块类型
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    public static List<Type> FindDependedModuleTypes(Type moduleType)
    {
        CheckXiHanModuleType(moduleType);

        List<Type> dependencies = [];

        var dependencyDescriptors = moduleType.GetCustomAttributes()
            .OfType<IDependedTypesProvider>();

        foreach (var descriptor in dependencyDescriptors)
        {
            foreach (var dependedModuleType in descriptor.GetDependedTypes())
            {
                _ = dependencies.AddIfNotContains(dependedModuleType);
            }
        }

        return dependencies;
    }

    /// <summary>
    /// 获取所有程序集
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    public static Assembly[] GetAllAssemblies(Type moduleType)
    {
        List<Assembly> assemblies = [];

        var additionalAssemblyDescriptors = moduleType.GetCustomAttributes()
            .OfType<IAdditionalModuleAssemblyProvider>();

        foreach (var descriptor in additionalAssemblyDescriptors)
        {
            foreach (var assembly in descriptor.GetAssemblies())
            {
                _ = assemblies.AddIfNotContains(assembly);
            }
        }

        assemblies.Add(moduleType.Assembly);

        return [.. assemblies];
    }

    /// <summary>
    /// 递归添加模块和依赖项，并以目录树形式打印
    /// </summary>
    /// <param name="moduleTypes">已处理的模块列表，避免重复</param>
    /// <param name="moduleType">当前模块类型</param>
    /// <param name="logger">日志记录器（可选）</param>
    /// <param name="prefix">前缀字符串，用于构造目录树分支</param>
    /// <param name="isLast">当前模块是否为同级中的最后一个</param>
    private static void AddModuleAndDependenciesRecursively(List<Type> moduleTypes, Type moduleType, ILogger? logger,
        string prefix = "", bool isLast = true)
    {
        // 检查是否是合法的 XiHan 模块类型
        CheckXiHanModuleType(moduleType);

        if (moduleTypes.Contains(moduleType))
        {
            // 构造当前节点的前缀和分支符号
            var nodeContainsLine = (string.IsNullOrEmpty(prefix) ? "" : prefix + (isLast ? "└─ " : "├─ ")) +
                                   moduleType.Namespace +
                                   "(此模块之前已加载)";
            // LogHelper.Handle(nodeContainsLine);
#pragma warning disable CA2254
            logger?.LogInformation(nodeContainsLine);
#pragma warning restore CA2254
            return;
        }

        // 构造当前节点的前缀和分支符号
        var nodeLine = (string.IsNullOrEmpty(prefix) ? "" : prefix + (isLast ? "└─ " : "├─ ")) + moduleType.Namespace;
        // LogHelper.Handle(nodeLine);
#pragma warning disable CA2254
        logger?.LogInformation(nodeLine);
#pragma warning restore CA2254

        moduleTypes.Add(moduleType);

        // 获取当前模块依赖的模块类型
        var dependedModuleTypes = FindDependedModuleTypes(moduleType);

        // 遍历每个依赖的模块，并递归调用
        for (var i = 0; i < dependedModuleTypes.Count; i++)
        {
            var childIsLast = i == dependedModuleTypes.Count - 1;
            // 为子节点构造新的前缀：如果当前节点是最后一个，则用空格，否则用竖线保持上层分支的连贯
            var childPrefix = prefix + (isLast ? "    " : "│   ");
            AddModuleAndDependenciesRecursively(moduleTypes, dependedModuleTypes[i], logger, childPrefix, childIsLast);
        }
    }

    /// <summary>
    /// 是否为模块
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsXiHanModule(Type type)
    {
        var typeInfo = type.GetTypeInfo();

        return typeInfo is { IsClass: true, IsAbstract: false, IsGenericType: false } &&
               typeof(IModule).GetTypeInfo().IsAssignableFrom(type);
    }

    /// <summary>
    /// 检测模块类
    /// </summary>
    /// <param name="moduleType"></param>
    /// <exception cref="ArgumentException"></exception>
    internal static void CheckXiHanModuleType(Type moduleType)
    {
        if (!IsXiHanModule(moduleType))
        {
            throw new ArgumentException("给定的类型不是模块:" + moduleType.AssemblyQualifiedName);
        }
    }
}
