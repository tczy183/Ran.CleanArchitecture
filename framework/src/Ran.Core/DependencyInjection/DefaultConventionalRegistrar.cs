namespace Ran.Core.DependencyInjection;

/// <summary>
/// 默认常规注册器
/// </summary>
public class DefaultConventionalRegistrar : ConventionalRegistrarBase
{
    /// <summary>
    /// 添加类型
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    public override void AddType(IServiceCollection services, Type type)
    {
        if (IsConventionalRegistrationDisabled(type))
        {
            return;
        }

        var dependencyAttribute = GetDependencyAttributeOrNull(type);
        var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

        if (lifeTime is null)
        {
            return;
        }

        var exposedServiceAndKeyedServiceTypes = GetExposedKeyedServiceTypes(type)
            .Concat(GetExposedServiceTypes(type).Select(t => new ServiceIdentifier(t)))
            .ToList();

        TriggerServiceExposing(services, type, exposedServiceAndKeyedServiceTypes);

        foreach (
            var serviceDescriptor in from exposedServiceType in exposedServiceAndKeyedServiceTypes
            let allExposingServiceTypes = exposedServiceType.ServiceKey is null
                ? exposedServiceAndKeyedServiceTypes.Where(x => x.ServiceKey is null).ToList()
                : exposedServiceAndKeyedServiceTypes
                    .Where(x =>
                        x.ServiceKey?.ToString() == exposedServiceType.ServiceKey?.ToString()
                    )
                    .ToList()
            select CreateServiceDescriptor(
                type,
                exposedServiceType.ServiceKey,
                exposedServiceType.ServiceType,
                allExposingServiceTypes,
                lifeTime.Value
            )
        )
        {
            if (dependencyAttribute?.ReplaceServices == true)
            {
                _ = services.Replace(serviceDescriptor);
            }
            else if (dependencyAttribute?.TryRegister == true)
            {
                services.TryAdd(serviceDescriptor);
            }
            else
            {
                services.Add(serviceDescriptor);
            }
        }
    }
}
