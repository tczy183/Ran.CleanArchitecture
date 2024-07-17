namespace Domain.Repositories;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IGenericRepository<T,TKey>  where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     持久化实体到数据库
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}