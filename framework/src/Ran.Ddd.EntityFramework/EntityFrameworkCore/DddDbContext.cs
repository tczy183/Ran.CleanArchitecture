using Microsoft.EntityFrameworkCore;
using Ran.Ddd.Domain.Abstraction.Repositories;

namespace Ran.Ddd.EntityFramework.EntityFrameworkCore;

public abstract class DddDbContext<TDbContext> : DbContext, IUnitOfWork
    where TDbContext : DbContext
{
    protected DddDbContext(DbContextOptions<TDbContext> options)
        : base(options) { }

    public virtual async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) > 0;
    }
}
