using Ran.Serilog.Sinks.Mysql.Sinks;
using Ran.Serilog.Sinks.Mysql.Sinks.ColumnWriters;
using Ran.Serilog.Sinks.Mysql.Sinks.EventArgs;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Ran.Serilog.Sinks.Mysql;

public static class LoggerConfigurationMySqlExtensions
{
    /// <summary>
    ///     The default batch size limit.
    /// </summary>
    private const int DefaultBatchSizeLimit = 30;

    /// <summary>
    /// The default queue limit.
    /// </summary>
    private const int DefaultQueueLimit = int.MaxValue;

    /// <summary>
    ///     Default time to wait between checking for event batches.
    /// </summary>
    public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(5);

    public static LoggerConfiguration MySql(
        this LoggerSinkConfiguration loggerConfiguration,
        string connectionString,
        IFormatProvider? formatProvider = null,
        string tableName = "Logs",
        Action<Exception>? failureCallback = null,
        IDictionary<string, ColumnWriterBase>? columnOptions = null,
        bool needAutoCreateTable = false,
        bool storeTimestampInUtc = false,
        Action<CreateTableEventArgs>? onCreateTable = null,
        uint batchSize = 100,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        LoggingLevelSwitch? levelSwitch = null
    )
    {
        ArgumentNullException.ThrowIfNull(loggerConfiguration);

        var option = GetOption(
            connectionString,
            formatProvider,
            tableName,
            failureCallback,
            columnOptions,
            needAutoCreateTable,
            storeTimestampInUtc,
            onCreateTable
        );

        if (batchSize is < 1 or > 1000)
            throw new ArgumentOutOfRangeException(
                "[batchSize] argument must be between 1 and 1000 inclusive"
            );

        try
        {
            return loggerConfiguration.Sink(
                new MySqlSink(option, batchSize),
                restrictedToMinimumLevel,
                levelSwitch
            );
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine(ex.Message);

            throw;
        }
    }

    internal static MySqlOption GetOption(
        string connectionString,
        IFormatProvider? formatProvider,
        string tableName,
        Action<Exception>? failureCallback,
        IDictionary<string, ColumnWriterBase>? columnOptions,
        bool needAutoCreateTable,
        bool storeTimestampInUtc,
        Action<CreateTableEventArgs> onCreateTable
    )
    {
        return new MySqlOption
        {
            ConnectionString = connectionString,
            FormatProvider = formatProvider,
            TableName = tableName,
            FailureCallback = failureCallback,
            ColumnOptions = columnOptions ?? new Dictionary<string, ColumnWriterBase>(),
            NeedAutoCreateTable = needAutoCreateTable,
            StoreTimestampInUtc = storeTimestampInUtc,
            OnCreateTable = onCreateTable,
        };
    }
}
