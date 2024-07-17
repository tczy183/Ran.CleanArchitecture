using Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFrameworkCore;

public class ApplicationReadDbContext(DbContextOptions<ApplicationReadDbContext> options)
    : DbContext(options), IApplicationReadDbContext
{
}