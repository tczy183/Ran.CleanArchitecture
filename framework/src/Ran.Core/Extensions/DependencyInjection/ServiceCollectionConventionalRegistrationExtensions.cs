﻿using Ran.Core.DependencyInjection;
using Ran.Core.Utils.System;

namespace Ran.Core.Extensions.DependencyInjection;

/// <summary>
/// 服务集合常规注册扩展方法
/// </summary>
public static class ServiceCollectionConventionalRegistrationExtensions
{
    /// <summary>
    /// 添加常规注册器
    /// </summary>
    /// <param name="services"></param>
    /// <param name="registrar"></param>
    /// <returns></returns>
    public static IServiceCollection AddConventionalRegistrar(
        this IServiceCollection services,
        IConventionalRegistrar registrar
    )
    {
        GetOrCreateRegistrarList(services).Add(registrar);
        return services;
    }

    /// <summary>
    /// 获取常规注册器
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static List<IConventionalRegistrar> GetConventionalRegistrars(
        this IServiceCollection services
    )
    {
        return GetOrCreateRegistrarList(services);
    }

    /// <summary>
    /// 获取或创建常规注册器
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static ConventionalRegistrarList GetOrCreateRegistrarList(IServiceCollection services)
    {
        var conventionalRegistrars = services
            .GetSingletonInstanceOrNull<IObjectAccessor<ConventionalRegistrarList>>()
            ?.Value;
        if (conventionalRegistrars is not null)
        {
            return conventionalRegistrars;
        }

        conventionalRegistrars = [new DefaultConventionalRegistrar()];
        _ = services.AddObjectAccessor(conventionalRegistrars);

        return conventionalRegistrars;
    }

    /// <summary>
    /// 添加泛型程序集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services)
    {
        return services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
    }

    /// <summary>
    /// 添加特定程序集
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        foreach (var registrar in services.GetConventionalRegistrars())
        {
            registrar.AddAssembly(services, assembly);
        }

        return services;
    }

    /// <summary>
    /// 添加类型
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IServiceCollection AddTypes(this IServiceCollection services, params Type[] types)
    {
        foreach (var registrar in services.GetConventionalRegistrars())
        {
            registrar.AddTypes(services, types);
        }

        return services;
    }

    /// <summary>
    /// 添加泛型类型
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddType<TType>(this IServiceCollection services)
    {
        return services.AddType(typeof(TType));
    }

    /// <summary>
    /// 添加类型
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IServiceCollection AddType(this IServiceCollection services, Type type)
    {
        foreach (var registrar in services.GetConventionalRegistrars())
        {
            registrar.AddType(services, type);
        }

        return services;
    }
}
