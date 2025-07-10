namespace Ran.Core.SimpleStateChecking;

/// <summary>
/// 简单状态检查器上下文
/// </summary>
public class SimpleStateCheckerContext<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 状态
    /// </summary>
    public TState State { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="state"></param>
    public SimpleStateCheckerContext(IServiceProvider serviceProvider, TState state)
    {
        ServiceProvider = serviceProvider;
        State = state;
    }
}
