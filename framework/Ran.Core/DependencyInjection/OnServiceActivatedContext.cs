namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务激活时的上下文
/// </summary>
public class OnServiceActivatedContext : IOnServiceActivatedContext
{
    /// <summary>
    /// 服务实例
    /// </summary>
    public object Instance { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="instance"></param>
    public OnServiceActivatedContext(object instance)
    {
        Instance = instance;
    }
}
