using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ran.Ddd.Domain.Abstraction.Entities;
using Ran.Ddd.Domain.Abstraction.Repositories;

namespace Ran.Ddd.EntityFramework.Repositories;

public class EfCoreRepository<TEntity, TKey, TDbContext>(TDbContext context)
    : IRepository<TEntity, TKey>
    where TEntity : notnull, Entity<TKey>
    where TKey : notnull
    where TDbContext : DbContext, IUnitOfWork
{
    public virtual IUnitOfWork UnitOfWork { get; } = context;
    protected virtual TDbContext DbContext { get; set; } = context;

    protected virtual DbSet<TEntity> GetDbSet()
    {
        return DbContext.Set<TEntity>();
    }

    protected virtual IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet().AsQueryable();
    }

    private protected virtual IQueryable<TEntity> WithDetails(
        Expression<Func<TEntity, object>>[]? propertySelectors = null
    )
    {
        var query = GetQueryable();
        return propertySelectors == null
            ? query
            : propertySelectors.Aggregate(
                query,
                (current, propertySelector) => current.Include(propertySelector)
            );
    }

    public TEntity Add(TEntity entity, bool autoSave = false)
    {
        var entityEntry = GetDbSet().Add(entity);
        if (autoSave)
        {
            DbContext.SaveChanges();
        }

        return entityEntry.Entity;
    }

    public async Task<TEntity> AddAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        var entityEntry = await GetDbSet().AddAsync(entity, cancellationToken);
        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return entityEntry.Entity;
    }

    public void AddRange(IEnumerable<TEntity> entities, bool autoSave = false)
    {
        GetDbSet().AddRange(entities);
        if (autoSave)
        {
            DbContext.SaveChanges();
        }
    }

    public async Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        await GetDbSet().AddRangeAsync(entities, cancellationToken);
        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public TEntity Update(TEntity entity, bool autoSave = false)
    {
        var entityEntry = GetDbSet().Update(entity);
        if (autoSave)
        {
            DbContext.SaveChanges();
        }

        return entityEntry.Entity;
    }

    public async Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        var entityEntry = GetDbSet().Update(entity);
        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return entityEntry.Entity;
    }

    public bool Remove(TEntity entity, bool autoSave = false)
    {
        GetDbSet().Remove(entity);
        if (autoSave)
        {
            DbContext.SaveChanges();
        }

        return true;
    }

    public async Task<bool> RemoveAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        GetDbSet().Remove(entity);
        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    public IEnumerable<TEntity> GetEntityList(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        string sorting = ""
    )
    {
        var query = includeDetails ? WithDetails(propertySelectors) : GetQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (!string.IsNullOrWhiteSpace(sorting))
        {
            query = query.OrderBy(sorting);
        }

        return query.ToList();
    }

    public async Task<IEnumerable<TEntity>> GetEntityListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        string sorting = "",
        CancellationToken cancellationToken = default
    )
    {
        var query = includeDetails ? WithDetails(propertySelectors) : GetQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (!string.IsNullOrWhiteSpace(sorting))
        {
            query = query.OrderBy(sorting);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public IEnumerable<TEntity> GetPagedEntityList(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        int pageIndex = 1,
        int pageSize = 20,
        string sorting = "",
        bool isPaging = true
    )
    {
        var query = includeDetails ? WithDetails(propertySelectors) : GetQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (!string.IsNullOrWhiteSpace(sorting))
        {
            query = query.OrderBy(sorting);
        }

        if (isPaging)
        {
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        return query.ToList();
    }

    public async Task<IEnumerable<TEntity>> GetPagedEntityListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool includeDetails = false,
        Expression<Func<TEntity, object>>[]? propertySelectors = null,
        int pageIndex = 1,
        int pageSize = 20,
        string sorting = "",
        bool isPaging = true,
        CancellationToken cancellationToken = default
    )
    {
        var query = includeDetails ? WithDetails(propertySelectors) : GetQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (!string.IsNullOrWhiteSpace(sorting))
        {
            query = query.OrderBy(sorting);
        }

        if (isPaging)
        {
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public int DeleteById(TKey id, bool autoSave = false)
    {
        var entity = Get(id);
        if (entity == null)
        {
            return 0;
        }

        GetDbSet().Remove(entity);
        if (autoSave)
        {
            DbContext.SaveChanges();
        }

        return 1;
    }

    public async Task<int> DeleteByIdAsync(
        TKey id,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await GetAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return 0;
        }

        GetDbSet().Remove(entity);
        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return 1;
    }

    public bool DeleteByIds(TKey[] keys, bool autoSave = false)
    {
        var entities = GetDbSet().Where(e => keys.Contains(e.Id)).ToList();
        if (entities.Count == 0)
        {
            return false;
        }

        GetDbSet().RemoveRange(entities);
        if (autoSave)
        {
            DbContext.SaveChanges();
        }

        return true;
    }

    public async Task<bool> DeleteByIdsAsync(
        TKey[] keys,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
    {
        var entities = await GetDbSet()
            .Where(e => keys.Contains(e.Id))
            .ToListAsync(cancellationToken);
        if (entities.Count == 0)
        {
            return false;
        }

        GetDbSet().RemoveRange(entities);
        if (autoSave)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    public TEntity? Get(TKey id, bool includeDetails = false)
    {
        return includeDetails
            ? WithDetails().FirstOrDefault(e => e.Id.Equals(id))
            : GetDbSet().Find(id);
    }

    public async Task<TEntity?> GetAsync(
        TKey id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    )
    {
        return includeDetails
            ? await WithDetails().FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken)
            : await GetDbSet().FindAsync(new object[] { id }, cancellationToken);
    }
}
