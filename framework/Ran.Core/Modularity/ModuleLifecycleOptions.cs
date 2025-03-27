using Ran.Core.Collections;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块生命周期选项
/// </summary>
public class ModuleLifecycleOptions
{
    /// <summary>
    /// 贡献者
    /// </summary>
    public ITypeList<IModuleLifecycleContributor> Contributors { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public ModuleLifecycleOptions()
    {
        Contributors = new TypeList<IModuleLifecycleContributor>();
    }
}
