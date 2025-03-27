namespace Ran.Core.DependencyInjection;

/// <summary>
/// 常规注册器接口
/// </summary>
public interface IConventionalRegistrar
{
    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    void AddAssembly(IServiceCollection services, Assembly assembly);

    /// <summary>
    /// 添加多个类型
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    void AddTypes(IServiceCollection services, params Type[] types);

    /// <summary>
    /// 添加类型
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    void AddType(IServiceCollection services, Type type);
}
