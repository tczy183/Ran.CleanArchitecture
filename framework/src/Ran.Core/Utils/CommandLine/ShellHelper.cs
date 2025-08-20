﻿using System.Diagnostics;

namespace Ran.Core.Utils.CommandLine;

/// <summary>
/// ShellHelper
/// </summary>
public static class ShellHelper
{
    /// <summary>
    /// Unix 系统命令
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static string Bash(string command)
    {
        var output = string.Empty;
        var escapedArgs = command.Replace(@"""", @"\""");

        ProcessStartInfo info = new()
        {
            FileName = @"/bin/bash",
            // /bin/bash -c 后面接命令 ，而 /bin/bash 后面接执行的脚本
            Arguments = $"""
                -c "{escapedArgs}"
                """,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = Process.Start(info);
        if (process is null)
        {
            return output;
        }

        output = process.StandardOutput.ReadToEnd();
        return output;
    }

    /// <summary>
    /// Windows 系统命令
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string Cmd(string fileName, string args)
    {
        var output = string.Empty;

        ProcessStartInfo info = new()
        {
            FileName = fileName,
            Arguments = args,
            RedirectStandardOutput = true,
            // 指定是否使用操作系统的外壳程序来启动进程，如果设置为 false，则使用 CreateProcess 函数直接启动进程
            UseShellExecute = false,
            // 指定是否在启动进程时创建一个新的窗口
            CreateNoWindow = true,
        };

        using var process = Process.Start(info);
        if (process is null)
        {
            return output;
        }

        output = process.StandardOutput.ReadToEnd();
        return output;
    }
}
