using Ran.Core.Utils.DataFilter.Paging.Dtos;
using Ran.Core.Utils.Reflections;

namespace Ran.Core.Utils.DataFilter.Paging.Handlers;

internal static class SortConditionParserCache
{
    /// <summary>
    /// 排序条件缓存
    /// </summary>
    public static readonly ConcurrentDictionary<string, LambdaExpression> Cache = new();
}

/// <summary>
/// 排序条件解析器
/// </summary>
public static class SortConditionParser<T>
{
    /// <summary>
    /// 获取排序条件解析器
    /// </summary>
    /// <param name="sortCondition"></param>
    /// <returns></returns>
    public static Expression<Func<T, object>> GetSortConditionParser(SortConditionDto sortCondition)
    {
        return GetSortConditionParser(sortCondition.SortField);
    }

    /// <summary>
    /// 获取排序条件解析器
    /// </summary>
    /// <param name="sortCondition"></param>
    /// <returns></returns>
    public static Expression<Func<T, object>> GetSortConditionParser(
        SortConditionDto<T> sortCondition
    )
    {
        return GetSortConditionParser(sortCondition.SortField);
    }

    /// <summary>
    /// 获取排序条件解析器
    /// </summary>
    /// <param name="propertyName">属性名称</param>s
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static Expression<Func<T, object>> GetSortConditionParser(string propertyName)
    {
        var type = typeof(T);
        var key = $"{type.FullName}.{propertyName}";
        if (SortConditionParserCache.Cache.TryGetValue(key, out var sortConditionParser))
        {
            return (Expression<Func<T, object>>)sortConditionParser;
        }

        var param = Expression.Parameter(type);
        var property = type.GetPropertyInfo(propertyName);
        var propertyAccess = Expression.MakeMemberAccess(param, property);

        // 将属性访问转换为 object 类型
        var converted = Expression.Convert(propertyAccess, typeof(object));
        sortConditionParser = Expression.Lambda<Func<T, object>>(converted, param);

        _ = SortConditionParserCache.Cache.TryAdd(key, sortConditionParser);

        return (Expression<Func<T, object>>)sortConditionParser;
    }
}
