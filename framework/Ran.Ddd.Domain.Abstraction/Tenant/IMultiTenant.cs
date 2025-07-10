using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Ddd.Domain.Abstraction.Tenant;

/// <summary>
/// 租户实体，直接使用GUid作为租户Id，避免多租户的复杂性
/// </summary>
public interface IMultiTenant : ITenant<Guid?>
{
    new Guid? TenantId { get; set; }
}
