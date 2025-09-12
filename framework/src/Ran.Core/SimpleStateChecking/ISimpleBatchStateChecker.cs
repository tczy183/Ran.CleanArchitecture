namespace Ran.Core.SimpleStateChecking;

/// <summary>
/// 简单批量状态检查器接口
/// </summary>
public interface ISimpleBatchStateChecker<TState> : ISimpleStateChecker<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    /// <summary>
    /// 是否启用
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(
        SimpleBatchStateCheckerContext<TState> context
    );
}
