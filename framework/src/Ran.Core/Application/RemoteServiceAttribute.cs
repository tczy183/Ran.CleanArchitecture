using Ran.Core.Utils.Reflections;

namespace Ran.Core.Application;

/// <summary>
/// 远程服务特性
/// </summary>
[Serializable]
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RemoteServiceAttribute : Attribute
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 是否启用元数据
    /// </summary>
    public bool IsMetadataEnabled { get; set; }

    /// <summary>
    /// 远程服务的组名。
    /// 一个模块的所有服务的组名预计应相同。
    /// 此名称也用于区分该组的服务端点。
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="isEnabled"></param>
    public RemoteServiceAttribute(bool isEnabled = true)
    {
        IsEnabled = isEnabled;
        IsMetadataEnabled = true;
    }

    /// <summary>
    /// 是否明确启用
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsExplicitlyEnabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo()
            .GetSingleAttributeOrNull<RemoteServiceAttribute>();
        return remoteServiceAttr is not null && remoteServiceAttr.IsEnabled;
    }

    /// <summary>
    /// 是否明确禁用
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsExplicitlyDisabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo()
            .GetSingleAttributeOrNull<RemoteServiceAttribute>();
        return remoteServiceAttr is not null && !remoteServiceAttr.IsEnabled;
    }

    /// <summary>
    /// 是否明确启用元数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsMetadataExplicitlyEnabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo()
            .GetSingleAttributeOrNull<RemoteServiceAttribute>();
        return remoteServiceAttr is not null && remoteServiceAttr.IsMetadataEnabled;
    }

    /// <summary>
    /// 是否明确禁用元数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsMetadataExplicitlyDisabledFor(Type type)
    {
        var remoteServiceAttr = type.GetTypeInfo()
            .GetSingleAttributeOrNull<RemoteServiceAttribute>();
        return remoteServiceAttr is not null && !remoteServiceAttr.IsMetadataEnabled;
    }

    /// <summary>
    /// 是否明确启用元数据
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
    {
        var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
        return remoteServiceAttr is not null && remoteServiceAttr.IsMetadataEnabled;
    }

    /// <summary>
    /// 是否明确禁用元数据
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
    {
        var remoteServiceAttr = method.GetSingleAttributeOrNull<RemoteServiceAttribute>();
        return remoteServiceAttr is not null && !remoteServiceAttr.IsMetadataEnabled;
    }
}
