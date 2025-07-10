using Ran.Core.Utils.CommandLine;
using Ran.Core.Utils.Logging;
using Ran.Core.Utils.System;
using Ran.Core.Utils.Timing;

namespace Ran.Core.Utils.HardwareInfos;

/// <summary>
/// 系统运行时间
/// </summary>
public static class RunningTimeHelper
{
    /// <summary>
    /// 处理器信息
    /// </summary>
    public static string RunningTime => GetRunningTime();

    /// <summary>
    /// 获取系统运行时间
    /// </summary>
    public static string GetRunningTime()
    {
        var runTime = string.Empty;

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var output = ShellHelper.Bash("uptime -s").Trim();
                var timeSpan = DateTime.Now - output.Trim().ParseToDateTime();
                runTime = timeSpan.FormatTimeSpanToString();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var output = ShellHelper.Bash("uptime | tail -n -1").Trim();
                // 提取运行时间部分
                var startIndex = output.IndexOf("up ", StringComparison.Ordinal) + 3;
                var endIndex = output.IndexOf(" user", StringComparison.Ordinal);
                var uptime = output[startIndex..endIndex].Trim();
                // 解析运行时间并转换为标准格式
                var uptimeSpan = ParseUptime(uptime);
                runTime = uptimeSpan.FormatTimeSpanToString();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime /Value").Trim();
                var outputArr = output.Split('=');
                if (outputArr.Length != 0)
                {
                    var timeSpan = DateTime.Now - outputArr[1].Split('.')[0].FormatStringToDate();
                    runTime = timeSpan.FormatTimeSpanToString();
                }
            }
        }
        catch (Exception ex)
        {
            LogHelper.Error("获取系统运行时间出错，" + ex.Message);
        }

        return runTime;
    }

    /// <summary>
    /// 解析运行时间
    /// </summary>
    /// <param name="uptime"></param>
    /// <returns></returns>
    private static TimeSpan ParseUptime(string uptime)
    {
        var parts = uptime.Split(',');
        int days = 0, hours = 0, minutes = 0;

        foreach (var part in parts)
        {
            var trimmedPart = part.Trim();

            if (trimmedPart.Contains("day"))
            {
                days = int.Parse(trimmedPart.Split(' ')[0]);
            }
            else if (trimmedPart.Contains(':'))
            {
                var timeParts = trimmedPart.Split(':');
                hours = int.Parse(timeParts[0]);
                minutes = int.Parse(timeParts[1]);
            }
        }

        return new TimeSpan(days, hours, minutes, 0);
    }
}
