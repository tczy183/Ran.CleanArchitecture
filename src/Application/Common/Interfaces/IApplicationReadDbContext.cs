namespace Application.Common.Interfaces;

public interface IApplicationReadDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}