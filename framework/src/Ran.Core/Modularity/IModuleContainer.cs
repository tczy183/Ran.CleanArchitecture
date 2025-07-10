namespace Ran.Core.Modularity;

/// <summary>
/// 模块容器接口
/// </summary>
public interface IModuleContainer
{
    /// <summary>
    /// 模块列表
    /// </summary>
    IReadOnlyList<IModuleDescriptor> Modules { get; }
}
