namespace Ran.Core.Utils.DataFilter.Paging.Dtos;

/// <summary>
/// 通用分页数据响应基类
/// </summary>
[Serializable]
public class PageDataDto
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PageDataDto() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="totalCount"></param>
    public PageDataDto(int totalCount)
    {
        var page = new PageInfoDto();
        PageInfo = page;
        TotalCount = totalCount;
        PageCount = (int)Math.Ceiling((decimal)totalCount / page.PageSize);
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="page"></param>
    /// <param name="totalCount"></param>
    public PageDataDto(PageInfoDto page, int totalCount)
    {
        PageInfo = page;
        TotalCount = totalCount;
        PageCount = (int)Math.Ceiling((decimal)totalCount / page.PageSize);
    }

    /// <summary>
    /// 分页数据
    /// </summary>
    public PageInfoDto PageInfo { get; set; } = new();

    /// <summary>
    /// 数据总数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int PageCount { get; set; }
}
