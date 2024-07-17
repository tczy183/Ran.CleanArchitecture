using Domain.Common;
using Domain.Repositories;
using Domain.Specifications;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFrameworkCore.Repositories;

public class EfRepository<T, TKey>(ApplicationWriteDbContext dbContext)
    : EfReadRepository<T, TKey>(dbContext), IRepository<T, TKey>
    where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
{
    public override async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);
    }

    public override async Task<List<T>> GetListAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification)
            .ToListAsync(cancellationToken);
    }

    public override async Task<T?> GetSingleOrDefaultAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public override async Task<int> CountAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).CountAsync(cancellationToken);
    }

    public override async Task<bool> AnyAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).AnyAsync(cancellationToken);
    }


    public virtual T Add(T entity)
    {
        DbSet.Add(entity);
        return entity;
    }

    public virtual void Update(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        DbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public virtual async Task<int> BatchDeleteAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet, specification).ExecuteDeleteAsync(cancellationToken);
    }
}