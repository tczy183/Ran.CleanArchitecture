﻿using MySql.Data.MySqlClient;
using Serilog.Events;

namespace Ran.Serilog.Sinks.Mysql.Sinks.ColumnWriters;

/// <summary>
///     This class contains the column writer base methods.
/// </summary>
public abstract class ColumnWriterBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ColumnWriterBase" /> class.
    /// </summary>
    /// <param name="dbType">The column type.</param>
    /// <param name="skipOnInsert">A value indicating whether the column in the insert queries is skipped or not.</param>
    /// <param name="order">
    /// The order of the column writer if needed.
    /// Is used for sorting the columns as the writers are ordered alphabetically per default.
    /// </param>
    protected ColumnWriterBase(MySqlDbType dbType, bool skipOnInsert = false, int? order = null)
    {
        this.DbType = dbType;
        this.SkipOnInsert = skipOnInsert;
        this.Order = order;
    }

    /// <summary>
    ///     Gets or sets the column type.
    /// </summary>
    /// <value>
    ///     The column type.
    /// </value>
    public MySqlDbType DbType { get; set; }

    /// <summary>
    /// Gets a value indicating whether the column in the insert queries is skipped or not.
    /// </summary>
    public bool SkipOnInsert { get; }

    /// <summary>
    /// Gets or sets the order of the column writer if needed.
    /// Is used for sorting the columns as the writers are ordered alphabetically per default.
    /// </summary>
    public int? Order { get; set; }

    /// <summary>
    ///     Gets the part of the log event to write to the column.
    /// </summary>
    /// <param name="logEvent">The log event.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>An object value.</returns>
    public abstract object? GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null);

    /// <summary>
    /// Gets the type of the SQL query.
    /// </summary>
    /// <returns>The MySql type for inserting it into the CREATE TABLE query.</returns>
    public virtual string GetSqlType()
    {
        return SqlTypeHelper.GetSqlTypeString(this.DbType);
    }
}
