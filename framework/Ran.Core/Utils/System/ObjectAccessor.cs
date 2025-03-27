namespace Ran.Core.Utils.System;

/// <summary>
/// 对象访问器接口
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectAccessor<T> : IObjectAccessor<T>
{
    /// <summary>
    /// 泛型对象
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public ObjectAccessor()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="obj"></param>
    public ObjectAccessor(T? obj)
    {
        Value = obj;
    }
}
