namespace Ran.Core.Modularity.PlugIns;

/// <summary>
/// 插件源接口
/// </summary>
public interface IPlugInSource
{
    /// <summary>
    /// 获取模块类型
    /// </summary>
    /// <returns></returns>
    Type[] GetModules();
}
