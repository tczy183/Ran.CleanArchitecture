using MySql.Data.MySqlClient;

namespace Ran.Serilog.Sinks.Mysql.Sinks;

public static class SqlTypeHelper
{
    /// <summary>
    ///     The default bit columns length.
    /// </summary>
    public const int DefaultBitColumnsLength = 8;

    /// <summary>
    ///     The default character columns length.
    /// </summary>
    public const int DefaultCharColumnsLength = 50;

    /// <summary>
    ///     The default varchar columns length.
    /// </summary>
    public const int DefaultVarcharColumnsLength = 50;

    public static string GetSqlTypeString(MySqlDbType dbType)
    {
        return dbType switch
        {
            MySqlDbType.Int32 => "INT",
            MySqlDbType.Int64 => "BIGINT",
            MySqlDbType.UInt32 => "INT",
            MySqlDbType.UInt64 => "BIGINT",
            MySqlDbType.Bit => $"BIT({DefaultBitColumnsLength})",
            MySqlDbType.VarChar => $"VARCHAR({DefaultVarcharColumnsLength})",
            MySqlDbType.String => $"CHAR({DefaultCharColumnsLength})",
            MySqlDbType.DateTime => "DATETIME",
            MySqlDbType.Date => "DATE",
            MySqlDbType.Time => "TIME",
            MySqlDbType.Float => "FLOAT",
            MySqlDbType.Double => "DOUBLE",
            MySqlDbType.Decimal => "DECIMAL",
            MySqlDbType.Byte => "TINYINT",
            MySqlDbType.UByte => "TINYINT",
            MySqlDbType.Binary => "BLOB",
            MySqlDbType.VarBinary => "VARBINARY",
            MySqlDbType.Blob => "BLOB",
            MySqlDbType.Guid => "CHAR(36)",
            MySqlDbType.LongText => "LONGTEXT",
            MySqlDbType.Text => "TEXT",
            MySqlDbType.MediumText => "MEDIUMTEXT",
            MySqlDbType.TinyText => "TINYTEXT",
            MySqlDbType.LongBlob => "LONGBLOB",
            MySqlDbType.MediumBlob => "MEDIUMBLOB",
            MySqlDbType.TinyBlob => "TINYBLOB",
            MySqlDbType.Timestamp => "TIMESTAMP",
            MySqlDbType.Year => "YEAR",
            MySqlDbType.Enum => "ENUM",
            MySqlDbType.Set => "SET",
            MySqlDbType.JSON => "JSON",
            MySqlDbType.Int16 => "SMALLINT",
            MySqlDbType.Int24 => "MEDIUMINT",
            MySqlDbType.Newdate => "DATE",
            MySqlDbType.VarString => $"VARCHAR({DefaultVarcharColumnsLength})",
            MySqlDbType.Vector => "GEOMETRY",
            MySqlDbType.NewDecimal => "DECIMAL",
            MySqlDbType.Geometry => "GEOMETRY",
            MySqlDbType.UInt16 => "SMALLINT",
            MySqlDbType.UInt24 => "MEDIUMINT",
            _ => throw new ArgumentOutOfRangeException(nameof(dbType), dbType, null),
        };
    }
}
