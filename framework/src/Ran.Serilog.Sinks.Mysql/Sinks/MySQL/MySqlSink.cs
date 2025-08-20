using System.Text;
using MySql.Data.MySqlClient;
using Ran.Serilog.Sinks.Mysql.Sinks.EventArgs;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace Ran.Serilog.Sinks.Mysql.Sinks.MySQL;

internal class MySqlSink : BatchProvider, ILogEventSink
{
    private readonly MySqlOption _option;

    public MySqlSink(MySqlOption option, uint batchSize = 100)
        : base((int)batchSize)
    {
        ArgumentNullException.ThrowIfNull(option, nameof(option));
        _option = option;
        ArgumentNullException.ThrowIfNull(option.ConnectionString, nameof(option.ConnectionString));
        ArgumentNullException.ThrowIfNull(option.TableName, nameof(option.TableName));

        if (_option.NeedAutoCreateTable)
        {
            var sqlConnection = GetSqlConnection();
            CreateTable(sqlConnection);
            _option.NeedAutoCreateTable = false;
            _option.OnCreateTable?.Invoke(new CreateTableEventArgs());
        }
    }

    public void Emit(LogEvent logEvent)
    {
        PushEvent(logEvent);
    }

    private MySqlConnection GetSqlConnection()
    {
        try
        {
            var conn = new MySqlConnection(_option.ConnectionString);
            conn.Open();

            return conn;
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine(ex.Message);

            throw;
        }
    }

    private MySqlCommand GetInsertCommand(MySqlConnection sqlConnection)
    {
        var tableCommandBuilder = new StringBuilder();
        tableCommandBuilder.Append($"INSERT INTO  {_option.TableName} (");
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
            tableCommandBuilder.Append($"CREATE TABLE IF NOT EXISTS {_option.TableName} (");

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

                insertCommand.Parameters["@ts"].Value = _option.StoreTimestampInUtc
                    ? logEvent.Timestamp.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fffzzz")
                    : logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fffzzz");

                insertCommand.Parameters["@level"].Value = logEvent.Level.ToString();
                insertCommand.Parameters["@template"].Value = logEvent.MessageTemplate.ToString();
                insertCommand.Parameters["@msg"].Value = logMessageString;
                insertCommand.Parameters["@ex"].Value = logEvent.Exception?.ToString();
                insertCommand.Parameters["@prop"].Value =
                    logEvent.Properties.Count > 0 ? logEvent.Properties.Json() : string.Empty;

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
