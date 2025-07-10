using Ran.Core.Utils.System;

namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务暴露时上下文
/// </summary>
public class OnServiceExposingContext : IOnServiceExposingContext
{
    /// <summary>
    /// 实现类型
    /// </summary>
    public Type ImplementationType { get; }

    /// <summary>
    /// 暴露的类型
    /// </summary>
    public List<ServiceIdentifier> ExposedTypes { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="implementationType"></param>
    /// <param name="exposedTypes"></param>
    public OnServiceExposingContext(Type implementationType, List<Type> exposedTypes)
    {
        ImplementationType = CheckHelper.NotNull(implementationType, nameof(implementationType));
        ExposedTypes = CheckHelper.NotNull(exposedTypes, nameof(exposedTypes))
            .ConvertAll(t => new ServiceIdentifier(t));
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="implementationType"></param>
    /// <param name="exposedTypes"></param>
    public OnServiceExposingContext(Type implementationType, List<ServiceIdentifier> exposedTypes)
    {
        ImplementationType = CheckHelper.NotNull(implementationType, nameof(implementationType));
        ExposedTypes = CheckHelper.NotNull(exposedTypes, nameof(exposedTypes));
    }
}
