using Ran.Core.Ran.Collections;
using Ran.Core.Ran.DynamicProxy;

namespace Ran.Core.Ran.DependencyInjection;

public interface IOnServiceRegistredContext
{
    ITypeList<IInterceptor> Interceptors { get; }

    Type ImplementationType { get; }
}
