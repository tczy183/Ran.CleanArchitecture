namespace Ran.Core.Modularity;

/// <summary>
/// 依赖类型提供器接口
/// </summary>
public interface IDependedTypesProvider
{
    /// <summary>
    /// 获取依赖类型
    /// </summary>
    /// <returns></returns>
    Type[] GetDependedTypes();
}
