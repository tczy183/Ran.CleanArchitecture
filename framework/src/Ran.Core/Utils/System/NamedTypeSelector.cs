namespace Ran.Core.Utils.System;

/// <summary>
/// 命名类型选择器
/// </summary>
public class NamedTypeSelector
{
    /// <summary>
    /// 选择器名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 断言
    /// </summary>
    public Func<Type, bool> Predicate { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="predicate">Predicate</param>
    public NamedTypeSelector(string name, Func<Type, bool> predicate)
    {
        Name = name;
        Predicate = predicate;
    }
}
