namespace Ran.Core.DependencyInjection;

/// <summary>
/// 依赖特性
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public sealed class DependencyAttribute : Attribute
{
    /// <summary>
    /// 生命周期
    /// </summary>
    public ServiceLifetime? Lifetime { get; set; }

    /// <summary>
    /// 是否尝试注册
    /// </summary>
    public bool TryRegister { get; set; }

    /// <summary>
    /// 是否替换服务
    /// </summary>
    public bool ReplaceServices { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public DependencyAttribute() { }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="lifetime"></param>
    public DependencyAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}
