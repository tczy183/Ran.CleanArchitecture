namespace Ran.Core.Utils.System;

/// <summary>
/// 空可释放对象
/// </summary>
public sealed class NullDisposable : IDisposable
{
    /// <summary>
    /// 实例
    /// </summary>
    public static NullDisposable Instance { get; } = new();

    /// <summary>
    /// 构造函数
    /// </summary>
    private NullDisposable()
    {
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
    }
}
