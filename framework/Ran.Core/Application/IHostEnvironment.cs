namespace Ran.Core.Application;

/// <summary>
/// 宿主环境接口
/// </summary>
public interface IHostEnvironment
{
    /// <summary>
    /// 环境名称
    /// </summary>
    string? EnvironmentName { get; set; }
}
