﻿using System.Diagnostics;

namespace Ran.Core.Utils.CommandLine;

/// <summary>
/// 脚本执行助手类，支持执行 .sh、.ps1、.bat 脚本
/// </summary>
public static class ScriptExecutor
{
    /// <summary>
    /// 执行脚本
    /// </summary>
    /// <param name="scriptFilePath">脚本文件的完整路径</param>
    /// <param name="arguments">传递给脚本的参数</param>
    /// <returns>执行结果（标准输出和标准错误）</returns>
    public static string ExecuteScript(string scriptFilePath, string arguments = "")
    {
        if (string.IsNullOrWhiteSpace(scriptFilePath) || !File.Exists(scriptFilePath))
        {
            throw new ArgumentException("脚本文件不存在", nameof(scriptFilePath));
        }

        var fileExtension = Path.GetExtension(scriptFilePath).ToLower();
        return fileExtension switch
        {
            ".sh" => ExecuteShellScript(scriptFilePath, arguments),
            ".ps1" => ExecutePowerShellScript(scriptFilePath, arguments),
            ".bat" => ExecuteBatchScript(scriptFilePath, arguments),
            _ => throw new NotSupportedException("不支持的脚本类型")
        };
    }

    /// <summary>
    /// 执行 Shell 脚本（Linux/macOS）
    /// </summary>
    /// <param name="scriptFilePath">脚本文件的完整路径</param>
    /// <param name="arguments">传递给脚本的参数</param>
    /// <returns>执行结果</returns>
    private static string ExecuteShellScript(string scriptFilePath, string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash", // Linux/macOS 使用 bash 执行脚本
            Arguments = $"{scriptFilePath} {arguments}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return ExecuteProcess(processStartInfo);
    }

    /// <summary>
    /// 执行 PowerShell 脚本（Windows）
    /// </summary>
    /// <param name="scriptFilePath">脚本文件的完整路径</param>
    /// <param name="arguments">传递给脚本的参数</param>
    /// <returns>执行结果</returns>
    private static string ExecutePowerShellScript(string scriptFilePath, string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "powershell", // Windows 使用 PowerShell 执行脚本
            Arguments = $"-ExecutionPolicy Bypass -File \"{scriptFilePath}\" {arguments}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return ExecuteProcess(processStartInfo);
    }

    /// <summary>
    /// 执行批处理脚本（Windows）
    /// </summary>
    /// <param name="scriptFilePath">脚本文件的完整路径</param>
    /// <param name="arguments">传递给脚本的参数</param>
    /// <returns>执行结果</returns>
    private static string ExecuteBatchScript(string scriptFilePath, string arguments)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = scriptFilePath, // Windows 使用 cmd 执行批处理脚本
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return ExecuteProcess(processStartInfo);
    }

    /// <summary>
    /// 执行进程并获取输出结果
    /// </summary>
    /// <param name="processStartInfo">进程启动信息</param>
    /// <returns>执行结果</returns>
    private static string ExecuteProcess(ProcessStartInfo processStartInfo)
    {
        var output = new StringBuilder();
        var error = new StringBuilder();

        try
        {
            using var process = Process.Start(processStartInfo);
            if (process is null)
            {
                throw new InvalidOperationException("无法启动进程");
            }

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data is not null)
                {
                    output.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data is not null)
                {
                    error.AppendLine(e.Data);
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"脚本执行失败，错误信息：{error}");
            }
        }
        catch (Exception ex)
        {
            return $"执行过程中发生错误：{ex.Message}";
        }

        return output.ToString();
    }
}
