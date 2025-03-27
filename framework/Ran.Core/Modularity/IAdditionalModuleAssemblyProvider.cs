namespace Ran.Core.Modularity;

/// <summary>
/// 附加模块组装提供器接口
/// </summary>
public interface IAdditionalModuleAssemblyProvider
{
    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <returns></returns>
    Assembly[] GetAssemblies();
}
