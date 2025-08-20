﻿namespace Ran.Core.Utils.Text.Json.Converters;

/// <summary>
/// DecimalJsonConverter
/// </summary>
public class DecimalJsonConverter : JsonConverter<decimal>
{
    /// <summary>
    /// 读
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override decimal Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDecimal();
        }
        else if (
            reader.TokenType == JsonTokenType.String
            && decimal.TryParse(reader.GetString(), out var value)
        )
        {
            return value;
        }

        return 0;
    }

    /// <summary>
    /// 写
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
