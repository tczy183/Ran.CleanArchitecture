namespace Ran.Basic.Authorization;

public sealed record DataPermission(
    Guid UserId,
    bool AllowsAll,
    bool AllowsSelf,
    IReadOnlySet<Guid> DepartmentIds
)
{
    public static DataPermission Denied(Guid userId) =>
        new(userId, AllowsAll: false, AllowsSelf: false, new HashSet<Guid>());
}
