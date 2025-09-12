using Microsoft.EntityFrameworkCore;

namespace Ran.Ddd.EntityFramework.EntityFrameworkCore;

public abstract class DddDbContext<TDbContext> : DbContext
    where TDbContext : DbContext { }
