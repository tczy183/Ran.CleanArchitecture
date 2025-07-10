namespace Ran.Core.DependencyInjection;

/// <summary>
/// 暴露键服务类型提供器接口
/// </summary>
public interface IExposedKeyedServiceTypesProvider
{
    /// <summary>
    /// 获取暴露的服务类型
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    ServiceIdentifier[] GetExposedServiceTypes(Type targetType);
}
