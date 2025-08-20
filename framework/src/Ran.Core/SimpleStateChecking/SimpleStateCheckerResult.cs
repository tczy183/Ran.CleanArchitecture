﻿namespace Ran.Core.SimpleStateChecking;

/// <summary>
/// 简单状态检查器结果
/// </summary>
public class SimpleStateCheckerResult<TState> : Dictionary<TState, bool>
    where TState : IHasSimpleStateCheckers<TState>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SimpleStateCheckerResult() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="states"></param>
    /// <param name="initValue"></param>
    public SimpleStateCheckerResult(IEnumerable<TState> states, bool initValue = true)
    {
        foreach (var state in states)
        {
            Add(state, initValue);
        }
    }
}
