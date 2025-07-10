using Ran.Core.Collections;
using Ran.Core.DynamicProxy;
using Ran.Core.Utils.System;

namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务注册时上下文
/// </summary>
public class OnServiceRegistredContext : IOnServiceRegistredContext
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public virtual ITypeList<IInterceptor> Interceptors { get; }

    /// <summary>
    /// 服务类型
    /// </summary>
    public virtual Type ServiceType { get; }

    /// <summary>
    /// 实现类型
    /// </summary>
    public virtual Type ImplementationType { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceType"></param>
    /// <param name="implementationType"></param>
    public OnServiceRegistredContext(Type serviceType, Type implementationType)
    {
        ServiceType = CheckHelper.NotNull(serviceType, nameof(serviceType));
        ImplementationType = CheckHelper.NotNull(implementationType, nameof(implementationType));

        Interceptors = new TypeList<IInterceptor>();
    }
}
