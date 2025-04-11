namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务激活上下文接口
/// </summary>
public interface IOnServiceActivatedContext
{
    /// <summary>
    /// 实例
    /// </summary>
    object Instance { get; }
}
