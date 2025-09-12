using Ran.Core.Utils.CommandLine;
using Ran.Core.Utils.Logging;
using Ran.Core.Utils.System;

namespace Ran.Core.Utils.HardwareInfos;

/// <summary>
/// 处理器帮助类
/// </summary>
public static class CpuHelper
{
    /// <summary>
    /// 处理器信息
    /// </summary>
    public static CpuInfo CpuInfos => GetCpuInfos();

    /// <summary>
    /// 获取处理器信息
    /// </summary>
    /// <returns></returns>
    public static CpuInfo GetCpuInfos()
    {
        CpuInfo cpuInfo = new()
        {
            CpuCount = Environment.ProcessorCount.ToString(),
            CpuRate = "0%",
        };

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var output = ShellHelper.Bash(@"top -b -n1 | grep ""Cpu(s)""").Trim();
                var lines = output.Split(',');
                if (lines.Length != 0)
                {
                    var loadPercentage = lines[3].Trim().Split(' ')[0];
                    cpuInfo.CpuRate = loadPercentage.ParseToLong() + "%";
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var output = ShellHelper
                    .Bash(@"top -l 1 -F | awk '/CPU usage/ {gsub(""%"", """"); print $7}'")
                    .Trim();
                cpuInfo.CpuRate = output.ParseToLong() + "%";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage /Value").Trim();
                var lines = output.Split(Environment.NewLine);
                if (lines.Length != 0)
                {
                    var loadPercentage = lines
                        .First(s => s.StartsWith("LoadPercentage"))
                        .Split('=')[1];
                    cpuInfo.CpuRate = loadPercentage.ParseToLong() + "%";
                }
            }
        }
        catch (Exception ex)
        {
            LogHelper.Error("获取处理器信息出错，" + ex.Message);
        }

        return cpuInfo;
    }
}

/// <summary>
/// 处理器信息
/// </summary>
public record CpuInfo
{
    /// <summary>
    /// 处理器个数
    /// </summary>
    public string CpuCount { get; set; } = string.Empty;

    /// <summary>
    /// 处理器使用占比
    /// </summary>
    public string CpuRate { get; set; } = string.Empty;
}
