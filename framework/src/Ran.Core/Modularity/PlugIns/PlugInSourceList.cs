namespace Ran.Core.Modularity.PlugIns;

/// <summary>
/// 插件源列表
/// </summary>
public class PlugInSourceList : List<IPlugInSource>
{
    /// <summary>
    /// 获取所有模块
    /// </summary>
    /// <param name="logger"></param>
    /// <returns></returns>
    internal Type[] GetAllModules(ILogger logger)
    {
        return this
            .SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies(logger))
            .Distinct()
            .ToArray();
    }
}
