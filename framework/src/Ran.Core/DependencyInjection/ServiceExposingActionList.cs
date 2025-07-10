namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务暴露时操作列表
/// </summary>
public class ServiceExposingActionList : List<Action<IOnServiceExposingContext>>;
