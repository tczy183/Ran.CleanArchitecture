using Ran.Core.Modularity.PlugIns;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块加载器接口
/// </summary>
public interface IModuleLoader
{
    /// <summary>
    /// 加载模块
    /// </summary>
    /// <param name="services"></param>
    /// <param name="startupModuleType"></param>
    /// <param name="plugInSources"></param>
    /// <returns></returns>
    IModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType,
        PlugInSourceList plugInSources);
}
