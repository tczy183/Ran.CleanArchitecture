namespace Ran.Core.DynamicProxy;

/// <summary>
/// 方法调用接口
/// </summary>
public interface IMethodInvocation
{
    /// <summary>
    /// 参数
    /// </summary>
    object[] Arguments { get; }

    /// <summary>
    /// 参数字典
    /// </summary>
    IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }

    /// <summary>
    /// 泛型参数
    /// </summary>
    Type[] GenericArguments { get; }

    /// <summary>
    /// 目标对象
    /// </summary>
    object TargetObject { get; }

    /// <summary>
    /// 方法
    /// </summary>
    MethodInfo Method { get; }

    /// <summary>
    /// 返回值
    /// </summary>
    object ReturnValue { get; set; }

    /// <summary>
    /// 方法调用
    /// </summary>
    /// <returns></returns>
    Task ProceedAsync();
}
