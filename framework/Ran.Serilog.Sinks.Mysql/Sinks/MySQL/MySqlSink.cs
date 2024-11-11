using System.Collections.Concurrent;
using System.Text;
using MySql.Data.MySqlClient;
using Ran.Serilog.Sinks.Mysql.Sinks;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Ran.Serilog.Sinks.Mysql.Sinks;

internal class MySqlSink : BatchProvider, ILogEventSink
{
    private readonly string _connectionString;
    private readonly bool _storeTimestampInUtc;
    private readonly string _tableName;

    public MySqlSink(
        string connectionString,
        string tableName = "Logs",
        bool storeTimestampInUtc = false,
        uint batchSize = 100) : base((int)batchSize)
    {
        ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
        _connectionString = connectionString;
        _tableName = tableName;
        _storeTimestampInUtc = storeTimestampInUtc;

        var sqlConnection = GetSqlConnection();
        CreateTable(sqlConnection);
    }

    public void Emit(LogEvent logEvent)
    {
        PushEvent(logEvent);
    }

    private MySqlConnection GetSqlConnection()
    {
        try
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();

            return conn;
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine(ex.Message);

            return null;
        }
    }

    private MySqlCommand GetInsertCommand(MySqlConnection sqlConnection)
    {
        var tableCommandBuilder = new StringBuilder();
        tableCommandBuilder.Append($"INSERT INTO  {_tableName} (");
        tableCommandBuilder.Append("Timestamp, Level, Template, Message, Exception, Properties) ");
        tableCommandBuilder.Append("VALUES (@ts, @level,@template, @msg, @ex, @prop)");

        var cmd = sqlConnection.CreateCommand();
        cmd.CommandText = tableCommandBuilder.ToString();

        cmd.Parameters.Add(new MySqlParameter("@ts", MySqlDbType.VarChar));
        cmd.Parameters.Add(new MySqlParameter("@level", MySqlDbType.VarChar));
        cmd.Parameters.Add(new MySqlParameter("@template", MySqlDbType.VarChar));
        cmd.Parameters.Add(new MySqlParameter("@msg", MySqlDbType.VarChar));
        cmd.Parameters.Add(new MySqlParameter("@ex", MySqlDbType.VarChar));
        cmd.Parameters.Add(new MySqlParameter("@prop", MySqlDbType.VarChar));

        return cmd;
    }
    
    

    private void CreateTable(MySqlConnection sqlConnection)
    {
        try
        {
            var tableCommandBuilder = new StringBuilder();
            tableCommandBuilder.Append($"CREATE TABLE IF NOT EXISTS {_tableName} (");
            tableCommandBuilder.Append("id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,");
            tableCommandBuilder.Append("Timestamp VARCHAR(100),");
            tableCommandBuilder.Append("Level VARCHAR(15),");
            tableCommandBuilder.Append("Template TEXT,");
            tableCommandBuilder.Append("Message TEXT,");
            tableCommandBuilder.Append("Exception TEXT,");
            tableCommandBuilder.Append("Properties TEXT,");
            tableCommandBuilder.Append("_ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP)");

            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = tableCommandBuilder.ToString();
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine(ex.Message);
        }
    }

    protected override async Task<bool> WriteLogEventAsync(ICollection<LogEvent> logEventsBatch)
    {
        try
        {
            await using var sqlCon = GetSqlConnection();
            await using var tr = await sqlCon.BeginTransactionAsync().ConfigureAwait(false);
            var insertCommand = GetInsertCommand(sqlCon);
            insertCommand.Transaction = tr;
            var isNeedCommit = false;
            foreach (var logEvent in logEventsBatch)
            {
                var logMessageString = new StringWriter(new StringBuilder());
                logEvent.RenderMessage(logMessageString);

                insertCommand.Parameters["@ts"].Value = _storeTimestampInUtc
                    ? logEvent.Timestamp.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fffzzz")
                    : logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fffzzz");

                insertCommand.Parameters["@level"].Value = logEvent.Level.ToString();
                insertCommand.Parameters["@template"].Value = logEvent.MessageTemplate.ToString();
                insertCommand.Parameters["@msg"].Value = logMessageString;
                insertCommand.Parameters["@ex"].Value = logEvent.Exception?.ToString();
                insertCommand.Parameters["@prop"].Value = logEvent.Properties.Count > 0
                    ? logEvent.Properties.Json()
                    : string.Empty;

                await insertCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
            }

            if (isNeedCommit)
                await tr.CommitAsync();

            return true;
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine(ex.Message);

            return false;
        }
    }
}