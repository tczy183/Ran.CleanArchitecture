using Domain.Common;
using Domain.Repositories;
using Domain.Specifications;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFrameworkCore.Repositories;

public class EfReadRepository<T, TKey>(ApplicationReadDbContext dbContext)
    : EfGenericRepository<T, TKey>(dbContext), IReadRepository<T, TKey>
    where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
{
    protected readonly DbSet<T> DbSet = dbContext.Set<T>();

    public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public virtual async Task<List<T>> GetListAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet.AsNoTracking(), specification)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetSingleOrDefaultAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet.AsNoTracking(), specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet.AsNoTracking(), specification).CountAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(ISpecification<T>? specification = null,
        CancellationToken cancellationToken = default)
    {
        return await SpecificationEvaluator.GetQuery(DbSet.AsNoTracking(), specification).AnyAsync(cancellationToken);
    }
}