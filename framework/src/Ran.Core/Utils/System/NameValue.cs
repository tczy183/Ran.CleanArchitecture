namespace Ran.Core.Utils.System;

/// <summary>
/// 可用于存储名称/值（或键/值）对
/// </summary>
[Serializable]
public class NameValue : NameValue<string>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public NameValue()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public NameValue(string name, string value)
    {
        Name = name;
        Value = value;
    }
}

/// <summary>
/// 可用于存储名称/值（或键/值）对
/// </summary>
[Serializable]
public class NameValue<T>
{
    /// <summary>
    /// 键
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 值
    /// </summary>
    public T Value { get; set; } = default!;

    /// <summary>
    /// 构造函数
    /// </summary>
    public NameValue()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public NameValue(string name, T value)
    {
        Name = name;
        Value = value;
    }
}
