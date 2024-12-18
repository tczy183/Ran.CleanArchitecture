namespace Ran.Core.Ran.DynamicProxy;

public interface IInterceptor
{
    Task InterceptAsync(IMethodInvocation invocation);
}
