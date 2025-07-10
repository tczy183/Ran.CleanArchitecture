namespace Ran.Ddd.Domain.Abstractions.Tenant;

public interface ITenant<TKey>
{
    TKey TenantId { get; set; }
}
