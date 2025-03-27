namespace Ran.Core.Application;

/// <summary>
/// 禁用功能特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DisableFeaturesAttribute : Attribute
{
    /// <summary>
    /// 将不会为该类注册任何拦截器
    /// 这将导致所有依赖拦截器的功能无法工作
    /// </summary>
    public bool DisableInterceptors { get; set; } = true;

    /// <summary>
    /// 中间件将跳过该类
    /// 这将导致所有依赖中间件的功能无法工作
    /// </summary>
    public bool DisableMiddleware { get; set; } = true;

    /// <summary>
    /// 不会为该类移除所有内置过滤器
    /// 这将导致所有依赖过滤器的功能无法工作
    /// </summary>
    public bool DisableMvcFilters { get; set; } = true;
}
