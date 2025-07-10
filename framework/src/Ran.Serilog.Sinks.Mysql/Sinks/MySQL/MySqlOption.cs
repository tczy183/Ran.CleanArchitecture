using Ran.Serilog.Sinks.Mysql.Sinks.ColumnWriters;
using Ran.Serilog.Sinks.Mysql.Sinks.EventArgs;

namespace Ran.Serilog.Sinks.Mysql;

public class MySqlOption
{
    /// <summary>
    ///     Gets or sets the connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the format provider.
    /// </summary>
    public IFormatProvider? FormatProvider { get; set; }

    /// <summary>
    ///     Gets or sets the table name.
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    ///  Gets or sets the failure callback.
    /// </summary>
    public Action<Exception>? FailureCallback { get; set; }

    /// <summary>
    ///     Gets or sets the column options.
    /// </summary>
    public IDictionary<string, ColumnWriterBase> ColumnOptions { get; set; } =
        new Dictionary<string, ColumnWriterBase>();

    /// <summary>
    /// Gets or sets a value indicating whether the table should be auto-created if it does not already exist or not.
    /// </summary>
    public bool NeedAutoCreateTable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the timestamp should be stored in UTC format.
    /// </summary>
    public bool StoreTimestampInUtc { get; set; }

    /// <summary>
    ///  Gets or sets the create table callback.
    /// </summary>
    public Action<CreateTableEventArgs>? OnCreateTable { get; set; }
}
