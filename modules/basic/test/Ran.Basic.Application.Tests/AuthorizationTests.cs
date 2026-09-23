using Ran.Basic.Authorization;
using Ran.Basic.Entities;
using Xunit;

namespace Ran.Basic.Application.Tests;

public sealed class AuthorizationTests
{
    private static readonly Guid UserId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid RootDepartmentId = Guid.Parse(
        "20000000-0000-0000-0000-000000000001"
    );
    private static readonly Guid ChildDepartmentId = Guid.Parse(
        "20000000-0000-0000-0000-000000000002"
    );

    [Fact]
    public async Task Direct_user_denial_should_override_role_grant()
    {
        var store = new FakeAuthorizationStore
        {
            User = CreateUser(),
            IsPermissionEnabled = true,
            UserPermissionGrant = false,
            RolePermissionGrant = true,
        };
        var checker = new PermissionChecker(store);

        var result = await checker.IsGrantedAsync(UserId, BasicPermissionNames.Users.Default);

        Assert.False(result);
    }

    [Fact]
    public async Task Role_grant_should_apply_when_user_has_no_override()
    {
        var store = new FakeAuthorizationStore
        {
            User = CreateUser(),
            IsPermissionEnabled = true,
            RolePermissionGrant = true,
        };
        var checker = new PermissionChecker(store);

        var result = await checker.IsGrantedAsync(UserId, BasicPermissionNames.Users.Default);

        Assert.True(result);
    }

    [Fact]
    public async Task Data_scope_should_merge_self_and_department_descendants()
    {
        var store = new FakeAuthorizationStore
        {
            User = CreateUser(),
            RoleDataGrants =
            [
                new RoleDataGrant(Guid.NewGuid(), DataScope.Self, []),
                new RoleDataGrant(Guid.NewGuid(), DataScope.DepartmentAndChildren, []),
            ],
            DepartmentAndDescendantIds = new HashSet<Guid> { RootDepartmentId, ChildDepartmentId },
        };
        var resolver = new DataPermissionResolver(store);

        var result = await resolver.ResolveAsync(UserId);

        Assert.False(result.AllowsAll);
        Assert.True(result.AllowsSelf);
        Assert.Equal(2, result.DepartmentIds.Count);
        Assert.Contains(ChildDepartmentId, result.DepartmentIds);
    }

    [Fact]
    public void Query_filter_should_allow_owned_or_department_data()
    {
        var permission = new DataPermission(
            UserId,
            AllowsAll: false,
            AllowsSelf: true,
            new HashSet<Guid> { RootDepartmentId }
        );
        var records = new[]
        {
            new SecuredRecord(UserId, null),
            new SecuredRecord(Guid.NewGuid(), RootDepartmentId),
            new SecuredRecord(Guid.NewGuid(), ChildDepartmentId),
        };

        var result = records.AsQueryable().WhereDataPermission(permission).ToList();

        Assert.Equal(2, result.Count);
    }

    private static BasicUser CreateUser()
    {
        var user = new BasicUser(UserId, "tester", "Tester", "tester@example.com");
        user.SetDepartment(RootDepartmentId);
        return user;
    }

    private sealed record SecuredRecord(Guid OwnerUserId, Guid? DataDepartmentId)
        : IDataPermissionEntity
    {
        public Guid? DepartmentId => DataDepartmentId;
    }

    private sealed class FakeAuthorizationStore : IBasicAuthorizationStore
    {
        public BasicUser? User { get; init; }

        public bool IsPermissionEnabled { get; init; }

        public bool? UserPermissionGrant { get; init; }

        public bool RolePermissionGrant { get; init; }

        public IReadOnlyList<RoleDataGrant> RoleDataGrants { get; init; } = [];

        public IReadOnlySet<Guid> DepartmentAndDescendantIds { get; init; } = new HashSet<Guid>();

        public Task<BasicUser?> FindUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default
        ) => Task.FromResult(User);

        public Task<bool> IsPermissionEnabledAsync(
            string permissionName,
            CancellationToken cancellationToken = default
        ) => Task.FromResult(IsPermissionEnabled);

        public Task<bool?> FindUserPermissionGrantAsync(
            Guid userId,
            string permissionName,
            CancellationToken cancellationToken = default
        ) => Task.FromResult(UserPermissionGrant);

        public Task<bool> IsPermissionGrantedToAnyRoleAsync(
            Guid userId,
            string permissionName,
            CancellationToken cancellationToken = default
        ) => Task.FromResult(RolePermissionGrant);

        public Task<IReadOnlyList<BasicMenu>> GetGrantedMenusAsync(
            Guid userId,
            CancellationToken cancellationToken = default
        ) => Task.FromResult<IReadOnlyList<BasicMenu>>([]);

        public Task<IReadOnlyList<RoleDataGrant>> GetRoleDataGrantsAsync(
            Guid userId,
            CancellationToken cancellationToken = default
        ) => Task.FromResult(RoleDataGrants);

        public Task<IReadOnlySet<Guid>> GetDepartmentAndDescendantIdsAsync(
            Guid departmentId,
            CancellationToken cancellationToken = default
        ) => Task.FromResult(DepartmentAndDescendantIds);

        public Task ReplaceUserRolesAsync(
            Guid userId,
            IReadOnlyCollection<Guid> roleIds,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException();

        public Task ReplaceRoleMenusAsync(
            Guid roleId,
            IReadOnlyCollection<Guid> menuIds,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException();

        public Task SetRoleDataScopeAsync(
            Guid roleId,
            DataScope dataScope,
            IReadOnlyCollection<Guid> departmentIds,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException();

        public Task SetPermissionGrantAsync(
            PermissionGrantProvider provider,
            Guid providerId,
            string permissionName,
            bool isGranted,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException();
    }
}
