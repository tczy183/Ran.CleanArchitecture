namespace Ran.Core.DependencyInjection;

/// <summary>
/// 属性注入服务接口
/// </summary>
public interface IInjectPropertiesService
{
    /// <summary>
    /// 注入属性
    /// </summary>
    TService InjectProperties<TService>(TService instance) where TService : notnull;

    /// <summary>
    /// 注入未设置的属性
    /// </summary>
    TService InjectUnsetProperties<TService>(TService instance) where TService : notnull;
}
