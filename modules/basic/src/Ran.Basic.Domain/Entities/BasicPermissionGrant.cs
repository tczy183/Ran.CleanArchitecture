using Ran.Basic.Authorization;
using Ran.Ddd.Domain.Abstraction.Entities;

namespace Ran.Basic.Entities;

public sealed class BasicPermissionGrant : Entity<Guid>
{
    private BasicPermissionGrant() { }

    public BasicPermissionGrant(
        Guid id,
        string permissionName,
        PermissionGrantProvider provider,
        Guid providerId,
        bool isGranted
    )
        : base(id)
    {
        PermissionName = BasicCheck.Required(permissionName, 256, nameof(permissionName));
        Provider = provider;
        ProviderKey = providerId.ToString("N");
        IsGranted = isGranted;
    }

    public string PermissionName { get; private set; } = string.Empty;

    public PermissionGrantProvider Provider { get; private set; }

    public string ProviderKey { get; private set; } = string.Empty;

    public bool IsGranted { get; private set; }

    public void SetGranted(bool isGranted) => IsGranted = isGranted;
}
