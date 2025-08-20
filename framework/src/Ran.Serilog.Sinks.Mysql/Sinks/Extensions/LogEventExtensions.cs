using System.Dynamic;
using System.Text.Json;
using Serilog.Events;

namespace Ran.Serilog.Sinks.Mysql;

internal static class LogEventExtensions
{
    internal static string Json(this LogEvent logEvent, bool storeTimestampInUtc = false)
    {
        return JsonSerializer.Serialize(ConvertToDictionary(logEvent, storeTimestampInUtc));
    }

    internal static IDictionary<string, object?> Dictionary(
        this LogEvent logEvent,
        bool storeTimestampInUtc = false,
        IFormatProvider? formatProvider = null
    )
    {
        return ConvertToDictionary(logEvent, storeTimestampInUtc, formatProvider);
    }

    internal static string Json(this IReadOnlyDictionary<string, LogEventPropertyValue> properties)
    {
        return JsonSerializer.Serialize(ConvertToDictionary(properties));
    }

    internal static IDictionary<string, object?> Dictionary(
        this IReadOnlyDictionary<string, LogEventPropertyValue> properties
    )
    {
        return ConvertToDictionary(properties);
    }

    private static IDictionary<string, object?> ConvertToDictionary(
        IReadOnlyDictionary<string, LogEventPropertyValue> properties
    )
    {
        var expObject = new Dictionary<string, object?>();
        foreach (var property in properties)
            expObject.Add(property.Key, Simplify(property.Value)!);

        return expObject;
    }

    private static IDictionary<string, object?> ConvertToDictionary(
        LogEvent logEvent,
        bool storeTimestampInUtc,
        IFormatProvider? formatProvider = null
    )
    {
        var eventObject = new Dictionary<string, object?>();
        eventObject.Add(
            "Timestamp",
            storeTimestampInUtc
                ? logEvent.Timestamp.ToUniversalTime().ToString("o")
                : logEvent.Timestamp.ToString("o")
        );

        eventObject.Add("LogLevel", logEvent.Level.ToString());
        eventObject.Add("LogMessageTemplate", logEvent.MessageTemplate.Text);
        eventObject.Add("LogMessage", logEvent.RenderMessage(formatProvider));
        eventObject.Add("LogException", logEvent.Exception);
        eventObject.Add("LogProperties", logEvent.Properties.Dictionary());

        return eventObject;
    }

    private static object Simplify(LogEventPropertyValue data)
    {
        switch (data)
        {
            case ScalarValue scalarValue:
                return scalarValue.Value ?? string.Empty;
            case IReadOnlyDictionary<string, LogEventPropertyValue> readOnlyDictionary:
                IDictionary<string, object?> dictionary = new ExpandoObject();
                foreach (string key in readOnlyDictionary.Keys)
                    dictionary.Add(key, Simplify(readOnlyDictionary[key]));
                return dictionary;
            case SequenceValue sequenceValue:
                return sequenceValue.Elements.Select(Simplify).ToArray();
            case StructureValue structureValue:
                try
                {
                    if (structureValue.TypeTag == null)
                        return structureValue.Properties.ToDictionary(
                            (Func<LogEventProperty, string>)(p => p.Name),
                            (Func<LogEventProperty, object>)(p => Simplify(p.Value))
                        );
                    if (
                        !structureValue.TypeTag.StartsWith("DictionaryEntry")
                        && !structureValue.TypeTag.StartsWith("KeyValuePair")
                    )
                        return structureValue.Properties.ToDictionary(
                            (Func<LogEventProperty, string>)(p => p.Name),
                            (Func<LogEventProperty, object>)(p => Simplify(p.Value))
                        );
                    object obj = Simplify(structureValue.Properties[0].Value);
                    var expandoObject = new ExpandoObject();
                    ((IDictionary<string, object?>)expandoObject).Add(
                        obj.ToString()!,
                        Simplify(structureValue.Properties[1].Value)
                    );
                    return expandoObject;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return string.Empty;
            default:
                return string.Empty;
        }
    }
}
