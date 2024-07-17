using Domain.Specifications;

namespace Domain.Repositories;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IRepository<T, TKey> : IReadRepository<T, TKey> where T : class,  IEntity<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     在数据库中添加实体
    /// </summary>
    /// <param name="entity">要添加的实体</param>
    /// <returns></returns>
    T Add(T entity);

    /// <summary>
    ///     在数据库中更新实体
    /// </summary>
    /// <param name="entity">要更新的实体</param>
    /// <returns></returns>
    void Update(T entity);

    /// <summary>
    ///     在数据库中删除实体
    /// </summary>
    /// <param name="entity">要删除的实体</param>
    /// <returns></returns>
    void Delete(T entity);

    /// <summary>
    ///     批量删除实体
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> BatchDeleteAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default);
}