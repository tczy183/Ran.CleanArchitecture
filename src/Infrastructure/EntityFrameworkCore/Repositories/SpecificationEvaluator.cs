using Domain.Common;
using Domain.Specifications;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFrameworkCore.Repositories;

public static class SpecificationEvaluator
{
    /// <summary>
    ///     获取查询对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="inputQuery">输入查询对象</param>
    /// <param name="specification">查询规范</param>
    /// <returns>查询对象</returns>
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T>? specification)
        where T : class, IEntity
    {
        if (specification == null) return inputQuery;

        var query = inputQuery;

        // 应用过滤条件
        if (specification.FilterCondition != null) query = query.Where(specification.FilterCondition);

        // 应用包含关系
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        // 应用字符串包含关系
        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        // 应用排序
        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);
        else if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);

        // 应用分组
        if (specification.GroupBy != null) query = query.GroupBy(specification.GroupBy).SelectMany(x => x);

        // 应用分页
        if (specification.IsPagingEnabled) query = query.Skip(specification.Skip).Take(specification.Take);
        return query;
    }
}