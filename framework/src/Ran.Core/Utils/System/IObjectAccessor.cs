namespace Ran.Core.Utils.System;

/// <summary>
/// 对象访问器接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IObjectAccessor<out T>
{
    /// <summary>
    /// 泛型对象
    /// </summary>
    T? Value { get; }
}
