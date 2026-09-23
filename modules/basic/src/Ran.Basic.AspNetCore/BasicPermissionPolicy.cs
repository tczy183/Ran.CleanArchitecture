namespace Ran.Basic.AspNetCore;

public static class BasicPermissionPolicy
{
    public const string DefaultPrefix = "Permission:";

    public static string For(string permissionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(permissionName);
        return DefaultPrefix + permissionName;
    }
}
