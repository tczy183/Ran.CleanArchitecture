namespace Ran.Core.Modularity;

/// <summary>
/// 模块描述器接口
/// </summary>
public interface IModuleDescriptor
{
    /// <summary>
    /// 模块类
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// 模块的主程序集
    /// </summary>
    Assembly Assembly { get; }

    /// <summary>
    /// 模块的所有组件
    /// 包括在模块 Type 上使用 AdditionalAssemblyAttribute 属性标记的主程序集和其他已定义的程序集
    /// </summary>
    Assembly[] AllAssemblies { get; }

    /// <summary>
    /// 模块类的实例(单例)
    /// </summary>
    IModule Instance { get; }

    /// <summary>
    /// 该模块是否作为插件加载
    /// </summary>
    bool IsLoadedAsPlugIn { get; }

    /// <summary>
    /// 此模块所依赖的模块
    /// 一个模块可以通过<see cref="DependsOnAttribute"/>属性依赖于另一个模块
    /// </summary>
    IReadOnlyList<IModuleDescriptor> Dependencies { get; }
}
