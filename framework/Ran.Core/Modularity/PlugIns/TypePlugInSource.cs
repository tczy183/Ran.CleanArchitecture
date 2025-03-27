namespace Ran.Core.Modularity.PlugIns;

/// <summary>
/// 按类型加载插件源
/// </summary>
public class TypePlugInSource : IPlugInSource
{
    /// <summary>
    /// 模块类型
    /// </summary>
    private readonly Type[] _moduleTypes;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="moduleTypes"></param>
    public TypePlugInSource(params Type[]? moduleTypes)
    {
        _moduleTypes = moduleTypes ?? [];
    }

    /// <summary>
    /// 获取模块类型
    /// </summary>
    /// <returns></returns>
    public Type[] GetModules()
    {
        return _moduleTypes;
    }
}
