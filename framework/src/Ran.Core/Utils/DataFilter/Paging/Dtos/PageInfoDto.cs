namespace Ran.Core.Utils.DataFilter.Paging.Dtos;

/// <summary>
/// 通用分页信息基类
/// </summary>
[Serializable]
public class PageInfoDto
{
    #region 默认值

    /// <summary>
    /// 默认当前页(防止非安全性传参)
    /// </summary>
    private const int DefaultIndex = 1;

    /// <summary>
    /// 默认每页大小(防止非安全性传参)
    /// </summary>
    private const int DefaultPageSize = 20;

    #endregion 默认值

    private readonly int _currentIndex = DefaultIndex;
    private readonly int _pageSize = DefaultPageSize;
    private readonly int[] _defaultPageSizeArray = [10, 20, 50, 100, 200, 500];

    /// <summary>
    /// 构造函数
    /// </summary>
    public PageInfoDto() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currentIndex"></param>
    /// <param name="pageSize"></param>
    public PageInfoDto(int currentIndex, int pageSize)
    {
        _currentIndex = currentIndex;
        _pageSize = pageSize;
    }

    /// <summary>
    /// 当前页标
    /// </summary>
    public int CurrentIndex
    {
        get => _currentIndex;
        init
        {
            if (value < DefaultIndex)
            {
                value = DefaultIndex;
            }

            _currentIndex = value;
        }
    }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        init
        {
            var defaultMaxPageSize = _defaultPageSizeArray.Max();
            var defaultMinPageSize = _defaultPageSizeArray.Min();

            value = value switch
            {
                _ when value > defaultMaxPageSize => defaultMaxPageSize,
                _ when value < defaultMinPageSize => defaultMinPageSize,
                // 不在默认每页大小数组中的值，取最接近的默认值
                _ => _defaultPageSizeArray.OrderBy(p => Math.Abs(p - value)).First(),
            };

            _pageSize = value;
        }
    }
}
