using Domain.Common;
using Domain.Repositories;

namespace Infrastructure.EntityFrameworkCore.Repositories;

public class EfGenericRepository<T, TKey>(ApplicationReadDbContext dbContext)
    : IGenericRepository<T, TKey> where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}