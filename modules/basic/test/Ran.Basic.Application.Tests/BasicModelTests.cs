using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Ran.Basic.Authorization;
using Ran.Basic.Entities;
using Ran.Basic.EntityFrameworkCore;
using Ran.Ddd.EntityFramework.EntityFrameworkCore;
using Xunit;

namespace Ran.Basic.Application.Tests;

public sealed class BasicModelTests
{
    [Fact]
    public void Model_should_include_access_control_entities_and_permission_seed_data()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(nameof(BasicModelTests))
            .Options;
        using var context = new TestDbContext(options);

        Assert.NotNull(context.Model.FindEntityType(typeof(BasicUser)));
        Assert.NotNull(context.Model.FindEntityType(typeof(BasicRole)));
        Assert.NotNull(context.Model.FindEntityType(typeof(BasicMenu)));
        var designTimeModel = context.GetService<IDesignTimeModel>().Model;
        var permission = designTimeModel.FindEntityType(typeof(BasicPermission));
        Assert.NotNull(permission);
        Assert.Equal(BasicPermissionNames.All.Count, permission.GetSeedData().Count());
    }

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options)
        : DddDbContext<TestDbContext>(options),
            IBasicDbContext
    {
        public DbSet<BasicUser> Users => Set<BasicUser>();

        public DbSet<BasicRole> Roles => Set<BasicRole>();

        public DbSet<BasicMenu> Menus => Set<BasicMenu>();

        public DbSet<BasicDepartment> Departments => Set<BasicDepartment>();

        public DbSet<BasicPermission> Permissions => Set<BasicPermission>();

        public DbSet<BasicPermissionGrant> PermissionGrants => Set<BasicPermissionGrant>();

        public DbSet<BasicUserRole> UserRoles => Set<BasicUserRole>();

        public DbSet<BasicRoleMenu> RoleMenus => Set<BasicRoleMenu>();

        public DbSet<BasicRoleDepartment> RoleDepartments => Set<BasicRoleDepartment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureBasicModule();
        }
    }
}
