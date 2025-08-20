using Ran.Core.Utils.CommandLine;
using Ran.Core.Utils.Logging;

namespace Ran.Core.Utils.HardwareInfos;

/// <summary>
/// 主板帮助类
/// </summary>
public static class BoardHelper
{
    /// <summary>
    /// 主板信息
    /// </summary>
    public static BoardInfo BoardInfos => GetBoardInfos();

    /// <summary>
    /// 获取主板信息
    /// </summary>
    /// <returns></returns>
    public static BoardInfo GetBoardInfos()
    {
        BoardInfo boardInfo = new();

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var output = ShellHelper.Bash("dmidecode -t baseboard").Trim();
                var lines = output
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();
                if (lines.Length != 0)
                {
                    boardInfo.Product = GetParmValue(lines, "Product Name", ':');
                    boardInfo.Manufacturer = GetParmValue(lines, "Manufacturer", ':');
                    boardInfo.SerialNumber = GetParmValue(lines, "Serial Number", ':');
                    boardInfo.Version = GetParmValue(lines, "Version", ':');
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var output = ShellHelper.Bash("system_profiler SPHardwareDataType").Trim();
                var lines = output
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();
                if (lines.Length != 0)
                {
                    boardInfo.Product = GetParmValue(lines, "Model Identifier", ':');
                    boardInfo.Manufacturer = GetParmValue(lines, "Chip", ':');
                    boardInfo.SerialNumber = GetParmValue(lines, "Serial Number (system)", ':');
                    boardInfo.Version = GetParmValue(lines, "Hardware UUID", ':');
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var output = ShellHelper
                    .Cmd("wmic", "baseboard get Product,Manufacturer,SerialNumber,Version /Value")
                    .Trim();
                var lines = output.Split(Environment.NewLine);
                if (lines.Length != 0)
                {
                    boardInfo.Product = GetParmValue(lines, "Product", '=');
                    boardInfo.Manufacturer = GetParmValue(lines, "Manufacturer", '=');
                    boardInfo.SerialNumber = GetParmValue(lines, "SerialNumber", '=');
                    boardInfo.Version = GetParmValue(lines, "Version", '=');
                }
            }
        }
        catch (Exception ex)
        {
            LogHelper.Error("获取主板信息出错，" + ex.Message);
        }

        return boardInfo;
    }

    private static string GetParmValue(string[] lines, string parm, char separator)
    {
        return lines.First(s => s.StartsWith(parm)).Split(separator)[1].Trim();
    }
}

/// <summary>
/// 主板信息
/// </summary>
public record BoardInfo
{
    /// <summary>
    /// 型号
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = string.Empty;
}
