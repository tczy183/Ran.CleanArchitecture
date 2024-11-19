using System.Linq.Expressions;
using Ran.Ddd.Domain.Abstractions.Entities;

namespace Ran.Ddd.Domain.Abstractions.Repositories;

/// <summary>
/// 仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IRepository<TEntity, in TKey>
    where TEntity : notnull, Entity<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 获取工作单元对象
    /// </summary>
    IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// 添加一个实体到仓储
    /// </summary>
    /// <param name="entity">要添加的实体对象</param>
    /// <param name="autoSave">自动保存</param>
    /// <returns></returns>
    TEntity Add(TEntity entity, bool autoSave = false);

    /// <summary>
    /// 添加实体到仓储
    /// </summary>
    /// <param name="entity">要添加的实体对象</param>
    /// <param name="autoSave">自动保存</param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task<TEntity> AddAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 批量添加实体到仓储
    /// </summary>
    /// <param name="entities">要添加的实体集合</param>
    /// <param name="autoSave">自动保存</param>
    /// <returns></returns>
    void AddRange(IEnumerable<TEntity> entities, bool autoSave = false);

    /// <summary>
    /// 批量添加实体到仓储的异步版本
    /// </summary>
    /// <param name="entities">要添加的实体集合</param>
    /// <param name="autoSave">自动保存</param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">要更新的实体对象</param>
    /// <param name="autoSave">自动保存</param>
    /// <returns></returns>
    TEntity Update(TEntity entity, bool autoSave = false);

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">要更新的实体对象</param>
    /// <param name="autoSave">自动保存</param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="entity">要删除的实体对象</param>
    /// <param name="autoSave">自动保存</param>
    /// <returns></returns>
    bool Remove(TEntity entity, bool autoSave = false);

    /// <summary>
    /// 要删除的实体对象
    /// </summary>
    /// <param name="entity">要删除的实体对象</param>
    /// <param name="autoSave">自动保存</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeDetails"></param>
    /// <param name="propertySelectors"></param>
    /// <param name="sorting"></param>
    /// <returns></returns>
    IEnumerable<TEntity> GetEntityList(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        string sorting = ""
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeDetails"></param>
    /// <param name="propertySelectors"></param>
    /// <param name="sorting"></param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetEntityListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        string sorting = "",
        CancellationToken cancellationToken = default
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeDetails"></param>
    /// <param name="propertySelectors"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sorting"></param>
    /// <param name="isPaging"></param>
    /// <returns></returns>
    IEnumerable<TEntity> GetPagedEntityList(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        int pageIndex = 1,
        int pageSize = 20,
        string sorting = "",
        bool isPaging = true
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeDetails"></param>
    /// <param name="propertySelectors"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sorting"></param>
    /// <param name="isPaging"></param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetPagedEntityListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        int pageIndex = 1,
        int pageSize = 20,
        string sorting = "",
        bool isPaging = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 根据主键删除实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <param name="autoSave">自动保存</param>
    /// <returns></returns>
    int DeleteById(TKey id, bool autoSave = false);

    /// <summary>
    /// 根据主键删除实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <param name="autoSave">自动保存</param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task<int> DeleteByIdAsync(
        TKey id,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 多个主键删除实体
    /// </summary>
    /// <param name="keys">主键值集合</param>
    /// <param name="autoSave">自动保存</param>
    /// <returns></returns>
    bool DeleteByIds(TKey[] keys, bool autoSave = false);

    /// <summary>
    /// 多个主键删除实体
    /// </summary>
    /// <param name="keys">主键值集合</param>
    /// <param name="autoSave">自动保存</param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task<bool> DeleteByIdsAsync(
        TKey[] keys,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <param name="includeDetails">是否级联</param>
    /// <returns></returns>
    TEntity? Get(TKey id, bool includeDetails = false);

    /// <summary>
    /// 根据ID获取实体
    /// </summary>
    /// <param name="id">主键值</param>
    /// <param name="includeDetails">是否级联</param>
    /// <param name="cancellationToken">取消操作token</param>
    /// <returns></returns>
    Task<TEntity?> GetAsync(
        TKey id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );
}
