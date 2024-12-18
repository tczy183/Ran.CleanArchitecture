namespace Ran.Core.Ran.DynamicProxy;

public abstract class Interceptor : IInterceptor
{
    public abstract Task InterceptAsync(IMethodInvocation invocation);
}
