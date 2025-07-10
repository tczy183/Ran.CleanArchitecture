namespace Ran.Core.DependencyInjection;

/// <summary>
/// 注册服务时的操作列表
/// </summary>
public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredContext>>
{
    /// <summary>
    /// 是否禁用类拦截器
    /// </summary>
    public bool IsClassInterceptorsDisabled { get; set; }
}
