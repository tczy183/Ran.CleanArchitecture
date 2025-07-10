namespace Ran.Core.Modularity;

/// <summary>
/// 类型依赖特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependsOnAttribute : Attribute, IDependedTypesProvider
{
    /// <summary>
    /// 依赖类型集合
    /// </summary>
    public Type[] DependedTypes { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dependedTypes"></param>
    public DependsOnAttribute(params Type[]? dependedTypes)
    {
        DependedTypes = dependedTypes ?? Type.EmptyTypes;
    }

    /// <summary>
    /// 获取依赖类型
    /// </summary>
    /// <returns></returns>
    public Type[] GetDependedTypes()
    {
        return DependedTypes;
    }
}
