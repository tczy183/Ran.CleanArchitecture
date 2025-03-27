namespace Ran.Core.Utils.Collections.Dtos;

/// <summary>
/// 树节点数据传输对象
/// </summary>
/// <typeparam name="T"></typeparam>
public class TreeNodeDto<T>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="value"></param>
    public TreeNodeDto(T value)
    {
        Value = value;
    }

    /// <summary>
    /// 节点值
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    public List<TreeNodeDto<T>> Children { get; set; } = [];
}
