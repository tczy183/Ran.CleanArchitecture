namespace Ran.Core.Utils.DataFilter.Paging.Dtos;

/// <summary>
/// 通用分页查询基类
/// </summary>
[Serializable]
public class PageQueryDto
{
    /// <summary>
    /// 是否查询所有数据
    /// 是则忽略分页信息，返回所有数据并绑定默认分页信息
    /// </summary>
    public bool? IsQueryAll { get; set; }

    /// <summary>
    /// 是否只返回分页信息
    /// 是则只返回分页信息，否则返回分页信息及结果数据
    /// </summary>
    public bool? IsOnlyPage { get; set; }

    /// <summary>
    /// 分页信息
    /// </summary>
    public PageInfoDto? PageInfo { get; set; }

    /// <summary>
    /// 选择条件集合
    /// </summary>
    public List<SelectConditionDto>? SelectConditions { get; set; }

    /// <summary>
    /// 排序条件集合
    /// </summary>
    public List<SortConditionDto>? SortConditions { get; set; }
}
