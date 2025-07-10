namespace Ran.Core.Reflection;

/// <summary>
/// 程序集查找器接口
/// </summary>
public interface IAssemblyFinder
{
    /// <summary>
    /// 获取程序集
    /// </summary>
    IReadOnlyList<Assembly> Assemblies { get; }
}
