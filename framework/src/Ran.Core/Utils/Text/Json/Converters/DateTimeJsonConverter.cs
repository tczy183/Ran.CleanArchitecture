namespace Ran.Core.Utils.Text.Json.Converters;

/// <summary>
/// DateTimeJsonConverter
/// </summary>
public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    private readonly string _dateFormatString;
    private readonly bool _isUtc;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DateTimeJsonConverter()
    {
        _dateFormatString = "yyyy-MM-dd HH:mm:ss";
        _isUtc = false;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dateFormatString"></param>
    /// <param name="isUtc"></param>
    public DateTimeJsonConverter(string dateFormatString, bool isUtc)
    {
        _dateFormatString = dateFormatString;
        _isUtc = isUtc;
    }

    /// <summary>
    /// 读
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            DateTime.TryParse(reader.GetString(), CultureInfo.CurrentCulture, out var time))
        {
            return _isUtc ? time.ToUniversalTime() : time;
        }

        return default;
    }

    /// <summary>
    /// 写
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_isUtc
            ? value.ToUniversalTime().ToString(_dateFormatString)
            : value.ToString(_dateFormatString));
    }
}

/// <summary>
/// DateTimeNullableConverter
/// </summary>
public class DateTimeNullableConverter : JsonConverter<DateTime?>
{
    private readonly string _dateFormatString;
    private readonly bool _isUtc;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DateTimeNullableConverter()
    {
        _dateFormatString = "yyyy-MM-dd HH:mm:ss";
        _isUtc = false;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dateFormatString"></param>
    /// <param name="isUtc"></param>
    public DateTimeNullableConverter(string dateFormatString, bool isUtc)
    {
        _dateFormatString = dateFormatString;
        _isUtc = isUtc;
    }

    /// <summary>
    /// 读
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            DateTime.TryParse(reader.GetString(), CultureInfo.CurrentCulture, out var time))
        {
            return _isUtc ? time.ToUniversalTime() : time;
        }

        return default;
    }

    /// <summary>
    /// 写
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(_isUtc
                ? value.Value.ToUniversalTime().ToString(_dateFormatString)
                : value.Value.ToString(_dateFormatString));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
