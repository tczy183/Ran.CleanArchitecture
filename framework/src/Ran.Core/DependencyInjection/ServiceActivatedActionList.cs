namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务激活时的动作列表
/// </summary>
public class ServiceActivatedActionList
    : List<KeyValuePair<ServiceDescriptor, Action<IOnServiceActivatedContext>>>
{
    /// <summary>
    /// 添加服务激活时的动作
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    public List<Action<IOnServiceActivatedContext>> GetActions(ServiceDescriptor descriptor)
    {
        return this.Where(x => x.Key == descriptor).Select(x => x.Value).ToList();
    }
}
