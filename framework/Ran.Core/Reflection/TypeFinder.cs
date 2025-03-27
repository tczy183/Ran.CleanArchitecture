using Ran.Core.Utils.Reflections;

namespace Ran.Core.Reflection;

/// <summary>
/// 类型查找器
/// </summary>
public class TypeFinder : ITypeFinder
{
    private readonly IAssemblyFinder _assemblyFinder;
    private readonly Lazy<IReadOnlyList<Type>> _types;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assemblyFinder"></param>
    public TypeFinder(IAssemblyFinder assemblyFinder)
    {
        _assemblyFinder = assemblyFinder;
        _types = new Lazy<IReadOnlyList<Type>>(FindAll, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    /// <summary>
    /// 类型
    /// </summary>
    public IReadOnlyList<Type> Types => _types.Value;

    /// <summary>
    /// 查找所有类型
    /// </summary>
    /// <returns></returns>
    private List<Type> FindAll()
    {
        List<Type> allTypes = [];

        foreach (var assembly in _assemblyFinder.Assemblies)
        {
            try
            {
                var typesInThisAssembly = AssemblyHelper.GetAllTypes(assembly);

                var inThisAssembly = typesInThisAssembly as Type[] ?? typesInThisAssembly.ToArray();
                if (inThisAssembly.Length == 0)
                {
                    continue;
                }

                allTypes.AddRange(inThisAssembly.Where(type => true));
            }
            catch
            {
                //TODO: Trigger a global event?
            }
        }

        return allTypes;
    }
}
