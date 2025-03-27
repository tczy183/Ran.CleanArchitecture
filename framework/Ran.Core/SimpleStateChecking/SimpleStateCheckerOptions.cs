using Ran.Core.Collections;

namespace Ran.Core.SimpleStateChecking;

/// <summary>
/// 简单状态检查器选项
/// </summary>
public class SimpleStateCheckerOptions<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    /// <summary>
    /// 全局状态检查器
    /// </summary>
    public ITypeList<ISimpleStateChecker<TState>> GlobalStateCheckers { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public SimpleStateCheckerOptions()
    {
        GlobalStateCheckers = new TypeList<ISimpleStateChecker<TState>>();
    }
}
