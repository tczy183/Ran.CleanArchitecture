using Ran.Core.Exceptions;

namespace Ran.Core.DependencyInjection;

/// <summary>
/// 暴露键值服务特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ExposeKeyedServiceAttribute<TServiceType>
    : Attribute,
        IExposedKeyedServiceTypesProvider
    where TServiceType : class
{
    /// <summary>
    /// 服务标识
    /// </summary>
    public ServiceIdentifier ServiceIdentifier { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceKey"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public ExposeKeyedServiceAttribute(object serviceKey)
    {
        if (serviceKey is null)
        {
            throw new UserFriendlyException(
                $"{nameof(serviceKey)} can not be null! Use {nameof(ExposeServicesAttribute)} instead."
            );
        }

        ServiceIdentifier = new ServiceIdentifier(serviceKey, typeof(TServiceType));
    }

    /// <summary>
    /// 获取暴露的服务类型
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public ServiceIdentifier[] GetExposedServiceTypes(Type targetType)
    {
        return [ServiceIdentifier];
    }
}
