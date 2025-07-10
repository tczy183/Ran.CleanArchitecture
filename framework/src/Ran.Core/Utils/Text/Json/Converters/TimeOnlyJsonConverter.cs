﻿namespace Ran.Core.Utils.Text.Json.Converters;

/// <summary>
/// TimeOnlyJsonConverter
/// </summary>
public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private readonly string _dateFormatString;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TimeOnlyJsonConverter()
    {
        _dateFormatString = "HH:mm:ss";
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dateFormatString"></param>
    public TimeOnlyJsonConverter(string dateFormatString)
    {
        _dateFormatString = dateFormatString;
    }

    /// <summary>
    /// 读
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            TimeOnly.TryParse(reader.GetString(), CultureInfo.CurrentCulture, out var time))
        {
            return time;
        }

        return default;
    }

    /// <summary>
    /// 写
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_dateFormatString));
    }
}

/// <summary>
/// TimeOnlyNullableConverter
/// </summary>
public class TimeOnlyNullableConverter : JsonConverter<TimeOnly?>
{
    private readonly string _dateFormatString;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TimeOnlyNullableConverter()
    {
        _dateFormatString = "HH:mm:ss";
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dateFormatString"></param>
    public TimeOnlyNullableConverter(string dateFormatString)
    {
        _dateFormatString = dateFormatString;
    }

    /// <summary>
    /// 读
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            TimeOnly.TryParse(reader.GetString(), CultureInfo.CurrentCulture, out var time))
        {
            return time;
        }

        return null;
    }

    /// <summary>
    /// 写
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString(_dateFormatString));
    }
}
