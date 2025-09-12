using Ran.Core.Utils.HardwareInfos;

namespace Ran.Core.Utils.Runtime;

/// <summary>
/// 操作系统帮助类
/// </summary>
public static class OsPlatformHelper
{
    /// <summary>
    /// 操作系统
    /// </summary>
    public static string OperatingSystem => GetOperatingSystem();

    /// <summary>
    /// 是否 Unix 系统
    /// </summary>
    public static bool OsIsUnix =>
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
        || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    /// 系统描述
    /// </summary>
    public static string OsDescription => RuntimeInformation.OSDescription;

    /// <summary>
    /// 系统版本
    /// </summary>
    public static string OsVersion => Environment.OSVersion.Version.ToString();

    /// <summary>
    /// 系统平台
    /// </summary>
    public static string Platform => Environment.OSVersion.Platform.ToString();

    /// <summary>
    /// 系统架构
    /// </summary>
    public static string OsArchitecture => RuntimeInformation.OSArchitecture.ToString();

    /// <summary>
    /// 系统目录
    /// </summary>
    public static string SystemDirectory => Environment.SystemDirectory;

    /// <summary>
    /// 运行时间
    /// </summary>
    public static string RunningTime => RunningTimeHelper.RunningTime;

    /// <summary>
    /// 交互模式
    /// </summary>
    public static string InteractiveMode => Environment.UserInteractive ? "交互运行" : "非交互运行";

    /// <summary>
    /// 获取操作系统
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string GetOperatingSystem()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? OSPlatform.OSX.ToString()
            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? OSPlatform.Linux.ToString()
            : RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OSPlatform.Windows.ToString()
            : throw new Exception("Cannot determine operating system!");
    }
}
