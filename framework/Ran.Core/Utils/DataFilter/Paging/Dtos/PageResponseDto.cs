namespace Ran.Core.Utils.DataFilter.Paging.Dtos;

/// <summary>
/// 通用分页响应基类
/// </summary>
[Serializable]
public class PageResponseDto
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PageResponseDto()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pageData"></param>
    public PageResponseDto(PageDataDto pageData)
    {
        PageData = pageData;
    }

    /// <summary>
    /// 分页数据
    /// </summary>
    public PageDataDto PageData { get; set; } = new();
}

/// <summary>
/// 通用分页响应泛型基类
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class PageResponseDto<T> : PageResponseDto
    where T : class, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PageResponseDto()
        : base()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pageData"></param>
    public PageResponseDto(PageDataDto pageData)
        : base(pageData)
    {
        ResponseData = null;
        ExtraData = null;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pageData"></param>
    /// <param name="responseData"></param>
    public PageResponseDto(PageDataDto pageData, IReadOnlyList<T>? responseData)
        : base(pageData)
    {
        ResponseData = responseData;
        ExtraData = null;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pageData"></param>
    /// <param name="responseData"></param>
    /// <param name="extraData"></param>
    public PageResponseDto(PageDataDto pageData, IReadOnlyList<T>? responseData, object extraData)
        : base(pageData)
    {
        ResponseData = responseData;
        ExtraData = extraData;
    }

    /// <summary>
    /// 数据列表（只读）
    /// </summary>
    public IReadOnlyList<T>? ResponseData { get; set; }

    /// <summary>
    /// 扩展数据（只读）
    /// </summary>
    public object? ExtraData { get; set; }
}
