namespace Ran.Core.Application;

/// <summary>
/// 应用服务类型
/// </summary>
/// <remarks><see cref="FlagsAttribute"/> 是为了方便使用位运算</remarks>
[Flags]
public enum ApplicationServiceTypes : byte
{
    /// <summary>
    /// 仅应用服务，不包含集成服务<see cref="IntegrationServiceAttribute"/>
    /// </summary>
    ApplicationServices = 1,

    /// <summary>
    /// 集成服务<see cref="IntegrationServiceAttribute"/>
    /// </summary>
    IntegrationServices = 2,

    /// <summary>
    /// 所有服务
    /// </summary>
    All = ApplicationServices | IntegrationServices
}
