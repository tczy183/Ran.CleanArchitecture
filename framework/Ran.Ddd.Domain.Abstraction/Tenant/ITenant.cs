namespace Ran.Ddd.Domain.Abstraction.Tenant;

public interface ITenant<TKey>
{
    TKey TenantId { get; set; }
}
