﻿using System.Dynamic;

namespace Ran.Core.Utils.Collections;

/// <summary>
/// 字典扩展方法
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// 如果字典中存在指定的键，则尝试获取其值
    /// </summary>
    /// <typeparam name="T">值的类型</typeparam>
    /// <param name="dictionary">字典对象</param>
    /// <param name="key">要查找的键</param>
    /// <param name="value">键对应的值，如果键不存在，则为默认值</param>
    /// <returns>如果字典中存在该键，则返回真(True)；否则返回假(False)</returns>
    internal static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T? value)
    {
        if (dictionary.TryGetValue(key, out var valueObj) && valueObj is T t)
        {
            value = t;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// 使用给定的键从字典中获取值。如果找不到，则返回默认值
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dictionary">要检查和获取的字典</param>
    /// <param name="key">要查找值的键</param>
    /// <returns>如果找到，返回值；如果找不到，返回默认值</returns>
    public static TValue? GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
    {
        return dictionary.GetValueOrDefault(key);
    }

    /// <summary>
    /// 使用给定的键从字典中获取值。如果找不到，则返回默认值
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dictionary">要检查和获取的字典</param>
    /// <param name="key">要查找值的键</param>
    /// <returns>如果找到，返回值；如果找不到，返回默认值</returns>
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.TryGetValue(key, out var obj) ? obj : default;
    }

    /// <summary>
    /// 使用给定的键从只读字典中获取值。如果找不到，则返回默认值
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dictionary">要检查和获取的只读字典</param>
    /// <param name="key">要查找值的键</param>
    /// <returns>如果找到，返回值；如果找不到，返回默认值</returns>
    public static TValue? GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.GetValueOrDefault(key);
    }

    /// <summary>
    /// 使用给定的键从并发字典中获取值。如果找不到，则返回默认值
    /// </summary>
    /// <typeparam name="TKey">键的类型，不能为空</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dictionary">要检查和获取的并发字典</param>
    /// <param name="key">要查找值的键</param>
    /// <returns>如果找到，返回值；如果找不到，返回默认值</returns>
    public static TValue? GetOrDefault<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
    {
        return dictionary.GetValueOrDefault(key);
    }

    /// <summary>
    /// 使用给定的键从字典中获取值。如果找不到，则使用工厂方法创建并添加值
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dictionary">要检查和获取的字典</param>
    /// <param name="key">要查找值的键</param>
    /// <param name="factory">如果字典中未找到，则用于创建值的工厂方法</param>
    /// <returns>如果找到，返回值；如果找不到，使用工厂方法创建并返回默认值</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TKey, TValue> factory)
    {
        return dictionary.TryGetValue(key, out var obj) ? obj : AddValue(dictionary, key, factory);
    }
    private static TValue AddValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
    {
        var value = factory(key);
        dictionary[key] = value;
        return value;
    }
    /// <summary>
    /// 使用给定的键从字典中获取值。如果找不到，则使用工厂方法创建并添加值
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dictionary">要检查和获取的字典</param>
    /// <param name="key">要查找值的键</param>
    /// <param name="factory">如果字典中未找到，则用于创建值的工厂方法</param>
    /// <returns>如果找到，返回值；如果找不到，使用工厂方法创建并返回默认值</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TValue> factory)
    {
        return dictionary.GetOrAdd(key, _ => factory());
    }

    /// <summary>
    /// 使用给定的键从并发字典中获取值。如果找不到，则使用工厂方法创建并添加值
    /// </summary>
    /// <typeparam name="TKey">键的类型，不能为空</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dictionary">要检查和获取的并发字典</param>
    /// <param name="key">要查找值的键</param>
    /// <param name="factory">如果并发字典中未找到，则用于创建值的工厂方法</param>
    /// <returns>如果找到，返回值；如果找不到，使用工厂方法创建并返回默认值</returns>
    public static TValue GetOrAdd<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key,
        Func<TValue> factory) where TKey : notnull
    {
        return dictionary.GetOrAdd(key, _ => factory());
    }

    /// <summary>
    /// 将字典转换为动态对象，以便在运行时添加和移除
    /// </summary>
    /// <param name="dictionary">要转换的字典对象</param>
    /// <returns>如果值正确，返回表示对象的 ExpandoObject</returns>
    public static dynamic ConvertToDynamicObject(this Dictionary<string, object> dictionary)
    {
        ExpandoObject expandoObject = new();
        ICollection<KeyValuePair<string, object>> expendObjectCollection = expandoObject!;

        foreach (var keyValuePair in dictionary)
        {
            expendObjectCollection.Add(keyValuePair);
        }

        return expandoObject;
    }
}
