﻿namespace Ran.Core.DependencyInjection;

/// <summary>
/// 服务标识符
/// </summary>
public readonly struct ServiceIdentifier : IEquatable<ServiceIdentifier>
{
    /// <summary>
    /// 服务 Key
    /// </summary>
    public object? ServiceKey { get; }

    /// <summary>
    /// 服务类型
    /// </summary>
    public Type ServiceType { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceType"></param>
    public ServiceIdentifier(Type serviceType)
    {
        ServiceType = serviceType;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceKey"></param>
    /// <param name="serviceType"></param>
    public ServiceIdentifier(object? serviceKey, Type serviceType)
    {
        ServiceKey = serviceKey;
        ServiceType = serviceType;
    }

    /// <summary>
    /// 是否相等
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ServiceIdentifier other)
    {
        return ServiceKey is null && other.ServiceKey is null
            ? ServiceType == other.ServiceType
            : ServiceKey is not null
                && other.ServiceKey is not null
                && ServiceType == other.ServiceType
                && ServiceKey.Equals(other.ServiceKey);
    }

    /// <summary>
    /// 是否相等
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return obj is ServiceIdentifier identifier && Equals(identifier);
    }

    /// <summary>
    /// 获取 HashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        if (ServiceKey is null)
        {
            return ServiceType.GetHashCode();
        }
        unchecked
        {
            return (ServiceType.GetHashCode() * 397) ^ ServiceKey.GetHashCode();
        }
    }

    /// <summary>
    /// 相等操作符
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(ServiceIdentifier left, ServiceIdentifier right) =>
        left.Equals(right);

    /// <summary>
    /// 不相等操作符
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(ServiceIdentifier left, ServiceIdentifier right) =>
        !(left == right);
}
