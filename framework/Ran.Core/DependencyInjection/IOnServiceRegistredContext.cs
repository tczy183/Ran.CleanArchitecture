using Ran.Core.Collections;
using Ran.Core.DynamicProxy;

namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务注册上下文接口
/// </summary>
public interface IOnServiceRegistredContext
{
    /// <summary>
    /// 服务拦截器列表
    /// </summary>
    ITypeList<IInterceptor> Interceptors { get; }

    /// <summary>
    /// 实现类型
    /// </summary>
    Type ImplementationType { get; }
}
