using System.Text.Json.Serialization.Metadata;

namespace Ran.Core.Utils.Text.Json.Serialization;

/// <summary>
/// 序列化扩展
/// </summary>
public static class SerializeExtensions
{
    /// <summary>
    /// 序列化为 Json
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static string SerializeTo(this object item)
    {
        return JsonSerializer.Serialize(item, JsonSerializerOptionsHelper.DefaultJsonSerializerOptions);
    }

    /// <summary>
    /// 反序列化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonString"></param>
    /// <returns></returns>
    public static T? DeserializeTo<T>(this string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString, JsonSerializerOptionsHelper.DefaultJsonSerializerOptions);
    }

    /// <summary>
    /// 反序列化为对象
    /// </summary>
    /// <param name="jsonString"></param>
    /// <returns></returns>
    public static object? DeserializeTo(this string jsonString)
    {
        return JsonSerializer.Deserialize(jsonString.ToStream(),
            JsonTypeInfo.CreateJsonTypeInfo<object>(JsonSerializerOptionsHelper.DefaultJsonSerializerOptions));
    }
}
