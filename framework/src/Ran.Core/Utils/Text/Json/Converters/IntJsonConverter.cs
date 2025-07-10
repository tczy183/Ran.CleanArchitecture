namespace Ran.Core.Utils.Text.Json.Converters;

/// <summary>
/// IntJsonConverter
/// </summary>
public class IntJsonConverter : JsonConverter<int>
{
    /// <summary>
    /// 读
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }
        else if (reader.TokenType == JsonTokenType.String && int.TryParse(reader.GetString(), out var value))
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
    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
