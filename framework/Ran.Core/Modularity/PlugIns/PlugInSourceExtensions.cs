using Ran.Core.Utils.System;

namespace Ran.Core.Modularity.PlugIns;

/// <summary>
/// 插件源扩展方法
/// </summary>
public static class PlugInSourceExtensions
{
    /// <summary>
    /// 获取所有模块和依赖项
    /// </summary>
    /// <param name="plugInSource"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static Type[] GetModulesWithAllDependencies(this IPlugInSource plugInSource, ILogger logger)
    {
        _ = CheckHelper.NotNull(plugInSource, nameof(plugInSource));

        return plugInSource.GetModules()
            .SelectMany(type => ModuleHelper.FindAllModuleTypes(type, logger))
            .Distinct().ToArray();
    }
}
