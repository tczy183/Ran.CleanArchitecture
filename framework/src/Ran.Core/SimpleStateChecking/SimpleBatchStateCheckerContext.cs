namespace Ran.Core.SimpleStateChecking;

/// <summary>
/// 简单批量状态检查器上下文
/// </summary>
public class SimpleBatchStateCheckerContext<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    /// <summary>
    /// 服务提供程序
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 状态
    /// </summary>
    public TState[] States { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="states"></param>
    public SimpleBatchStateCheckerContext(IServiceProvider serviceProvider, TState[] states)
    {
        ServiceProvider = serviceProvider;
        States = states;
    }
}
