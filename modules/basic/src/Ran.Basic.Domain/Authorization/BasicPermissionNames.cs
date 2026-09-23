namespace Ran.Basic.Authorization;

public static class BasicPermissionNames
{
    public const string GroupName = "Basic";

    public static class Users
    {
        public const string Default = GroupName + ".Users";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string ManageRoles = Default + ".ManageRoles";
        public const string ManagePermissions = Default + ".ManagePermissions";
    }

    public static class Roles
    {
        public const string Default = GroupName + ".Roles";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string ManageMenus = Default + ".ManageMenus";
        public const string ManagePermissions = Default + ".ManagePermissions";
        public const string ManageDataScope = Default + ".ManageDataScope";
    }

    public static class Menus
    {
        public const string Default = GroupName + ".Menus";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Departments
    {
        public const string Default = GroupName + ".Departments";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static IReadOnlyList<string> All { get; } =
        [
            GroupName,
            Users.Default,
            Users.Create,
            Users.Update,
            Users.Delete,
            Users.ManageRoles,
            Users.ManagePermissions,
            Roles.Default,
            Roles.Create,
            Roles.Update,
            Roles.Delete,
            Roles.ManageMenus,
            Roles.ManagePermissions,
            Roles.ManageDataScope,
            Menus.Default,
            Menus.Create,
            Menus.Update,
            Menus.Delete,
            Departments.Default,
            Departments.Create,
            Departments.Update,
            Departments.Delete,
        ];
}
