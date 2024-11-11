using Ran.Serilog.Sinks.Mysql.Sinks;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Ran.Serilog.Sinks.Mysql;

public static class LoggerConfigurationMySqlExtensions
{
    public static LoggerConfiguration MySql(
        this LoggerSinkConfiguration loggerConfiguration,
        string connectionString,
        string tableName = "Logs",
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        bool storeTimestampInUtc = false,
        uint batchSize = 100,
        LoggingLevelSwitch levelSwitch = null)
    {
        ArgumentNullException.ThrowIfNull(loggerConfiguration);

        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        if (batchSize is < 1 or > 1000)
            throw new ArgumentOutOfRangeException("[batchSize] argument must be between 1 and 1000 inclusive");

        try {
            return loggerConfiguration.Sink(
                new MySqlSink(connectionString, tableName, storeTimestampInUtc, batchSize),
                restrictedToMinimumLevel,
                levelSwitch);
        }
        catch (Exception ex) {
            SelfLog.WriteLine(ex.Message);

            throw;
        }
    }
}