using Microsoft.EntityFrameworkCore;

namespace Ran.Ddd.EntityFramework.EntityFrameworkCore;

public abstract class BaseDbContext<TDbContext> : DbContext
    where TDbContext : DbContext { }
