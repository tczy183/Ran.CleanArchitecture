namespace Ran.Core.Modularity;

/// <summary>
/// 附加程序集特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AdditionalAssemblyAttribute : Attribute, IAdditionalModuleAssemblyProvider
{
    /// <summary>
    /// 程序集类型
    /// </summary>
    public Type[] TypesInAssemblies { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="typesInAssemblies"></param>
    public AdditionalAssemblyAttribute(params Type[]? typesInAssemblies)
    {
        TypesInAssemblies = typesInAssemblies ?? Type.EmptyTypes;
    }

    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <returns></returns>
    public virtual Assembly[] GetAssemblies()
    {
        return TypesInAssemblies.Select(t => t.Assembly).Distinct().ToArray();
    }
}
