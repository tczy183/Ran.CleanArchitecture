namespace Ran.Core.SimpleStateChecking;

/// <summary>
/// 简单状态检查器接口
/// </summary>
public interface ISimpleStateChecker<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    /// <summary>
    /// 是否启用
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<bool> IsEnabledAsync(SimpleStateCheckerContext<TState> context);
}
