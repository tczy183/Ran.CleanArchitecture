using Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFrameworkCore;

public class ApplicationWriteDbContext(DbContextOptions<ApplicationReadDbContext> options)
    : ApplicationReadDbContext(options), IApplicationWriteDbContext;