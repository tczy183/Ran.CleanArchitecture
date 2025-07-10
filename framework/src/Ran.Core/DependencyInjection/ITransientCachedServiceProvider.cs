namespace Ran.Core.DependencyInjection;

/// <summary>
/// 通过缓存已解析的服务来提供服务
/// 它缓存包括瞬态在内的所有类型的服务
/// 此服务的生命周期是瞬态的
/// 有关具有作用域生命周期的服务，请参见 <see cref="ICachedServiceProvider"/>
/// </summary>
public interface ITransientCachedServiceProvider : ICachedServiceProviderBase;
