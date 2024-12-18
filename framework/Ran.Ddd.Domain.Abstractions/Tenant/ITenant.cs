namespace Ran.Ddd.Domain.Abstractions.Entities;

public interface ITenant<TKey>
{
    TKey TenantId { get; set; }
}
