﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Ran.Ddd.EntityFramework.Microsoft.EntityFrameworkCore;

public static class EntityFrameworkQueryableExtensions
{
    /// <summary>
    /// Specifies the related objects to include in the query results.
    /// </summary>
    /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include.</param>
    /// <param name="condition">A boolean value to determine to include <paramref name="path"/> or not.</param>
    /// <param name="path">The type of navigation property being included.</param>
    public static IQueryable<T> IncludeIf<T, TProperty>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, TProperty>> path
    )
        where T : class
    {
        return condition ? source.Include(path) : source;
    }
}
