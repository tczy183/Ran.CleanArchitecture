namespace Ran.Core.Utils.Text.Json.Converters;

/// <summary>
/// DateOnlyJsonConverter
/// </summary>
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private readonly string _dateFormatString;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DateOnlyJsonConverter()
    {
        _dateFormatString = "yyyy-MM-dd";
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dateFormatString"></param>
    public DateOnlyJsonConverter(string dateFormatString)
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
    public override DateOnly Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (
            reader.TokenType == JsonTokenType.String
            && DateOnly.TryParse(reader.GetString(), CultureInfo.CurrentCulture, out var date)
        )
        {
            return date;
        }

        return default;
    }

    /// <summary>
    /// 写
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_dateFormatString));
    }
}

/// <summary>
/// DateOnlyNullableConverter
/// </summary>
public class DateOnlyNullableConverter : JsonConverter<DateOnly?>
{
    private readonly string _dateFormatString;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DateOnlyNullableConverter()
    {
        _dateFormatString = "yyyy-MM-dd";
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dateFormatString"></param>
    public DateOnlyNullableConverter(string dateFormatString)
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
    public override DateOnly? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (
            reader.TokenType == JsonTokenType.String
            && DateOnly.TryParse(reader.GetString(), CultureInfo.CurrentCulture, out var date)
        )
        {
            return date;
        }

        return null;
    }

    /// <summary>
    /// 写
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(
        Utf8JsonWriter writer,
        DateOnly? value,
        JsonSerializerOptions options
    )
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString(_dateFormatString));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
