namespace Ran.Core.SimpleStateChecking;

/// <summary>
/// 具有简单状态检查器接口接口
/// </summary>
public interface IHasSimpleStateCheckers<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    /// <summary>
    /// 状态检查器列表
    /// </summary>
    List<ISimpleStateChecker<TState>> StateCheckers { get; }
}
