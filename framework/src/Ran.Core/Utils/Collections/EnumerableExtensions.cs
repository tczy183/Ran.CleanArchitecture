﻿namespace Ran.Core.Utils.Collections;

/// <summary>
/// 可列举扩展方法
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// 使用指定的分隔符连接构造的 <see cref="IEnumerable{T}"/> 集合（类型为 System.String）的成员
    /// 这是 string.Join(...) 的快捷方式
    /// </summary>
    /// <param name="source">包含要连接的字符串的集合</param>
    /// <param name="separator">要用作分隔符的字符串只有当 values 有多个元素时，separator 才会包含在返回的字符串中</param>
    /// <returns>由 values 的成员组成的字符串，这些成员由 separator 字符串分隔。如果 values 没有成员，则方法返回 System.String.Empty</returns>
    public static string JoinAsString(this IEnumerable<string> source, string separator)
    {
        return string.Join(separator, source);
    }

    /// <summary>
    /// 使用指定的分隔符连接集合的成员
    /// 这是 string.Join(...) 的快捷方式
    /// </summary>
    /// <param name="source">包含要连接的对象的集合</param>
    /// <param name="separator">要用作分隔符的字符串只有当 values 有多个元素时，separator 才会包含在返回的字符串中</param>
    /// <typeparam name="T">values 成员的类型</typeparam>
    /// <returns>由 values 的成员组成的字符串，这些成员由 separator 字符串分隔如果 values 没有成员，则方法返回 System.String.Empty</returns>
    public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
    {
        return string.Join(separator, source);
    }

    /// <summary>
    /// 如果给定的条件为真，则使用给定的谓词对 <see cref="IEnumerable{T}"/> 进行选择
    /// </summary>
    /// <param name="source">要应用选择的枚举对象</param>
    /// <param name="condition">第三方条件</param>
    /// <param name="predicate">用于选择枚举对象的谓词</param>
    /// <returns>基于 <paramref name="condition"/> 的选择或未选择的枚举对象</returns>
    public static IEnumerable<T> WhereIf<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<T, bool> predicate
    )
    {
        return condition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// 如果给定的条件为真，则使用给定的谓词对 <see cref="IEnumerable{T}"/> 进行选择
    /// </summary>
    /// <param name="source">要应用选择的枚举对象</param>
    /// <param name="condition">第三方条件</param>
    /// <param name="predicate">用于选择枚举对象的谓词，包含索引</param>
    /// <returns>基于 <paramref name="condition"/> 的选择或未选择的枚举对象</returns>
    public static IEnumerable<T> WhereIf<T>(
        this IEnumerable<T> source,
        bool condition,
        Func<T, int, bool> predicate
    )
    {
        return condition ? source.Where(predicate) : source;
    }
}
