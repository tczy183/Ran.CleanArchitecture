namespace Ran.Core.Application;

/// <summary>
/// 宿主环境
/// </summary>
public class HostEnvironment : IHostEnvironment
{
    /// <summary>
    /// 环境名称
    /// </summary>
    public string? EnvironmentName { get; set; }
}
