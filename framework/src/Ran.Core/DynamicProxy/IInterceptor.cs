namespace Ran.Core.DynamicProxy;

/// <summary>
/// 拦截器接口
/// </summary>
public interface IInterceptor
{
    /// <summary>
    /// 异步拦截
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    Task InterceptAsync(IMethodInvocation invocation);
}
