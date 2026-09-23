using Microsoft.EntityFrameworkCore;
using Ran.Basic.Authorization;
using Ran.Basic.Entities;

namespace Ran.Basic.EntityFrameworkCore;

public static class BasicModelBuilderExtensions
{
    public static ModelBuilder ConfigureBasicModule(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<BasicUser>(builder =>
        {
            builder.ToTable(BasicDbProperties.TablePrefix + "Users", BasicDbProperties.Schema);
            builder.HasKey(user => user.Id);
            builder.Property(user => user.UserName).HasMaxLength(64).IsRequired();
            builder.HasIndex(user => user.UserName).IsUnique();
            builder.Property(user => user.DisplayName).HasMaxLength(128).IsRequired();
            builder.Property(user => user.Email).HasMaxLength(256).IsRequired();
            builder.HasIndex(user => user.Email).IsUnique();
            builder.Property(user => user.PhoneNumber).HasMaxLength(32);
            builder.Property(user => user.PasswordHash).HasMaxLength(512);
            builder.Property(user => user.SecurityStamp).HasMaxLength(64).IsRequired();
            builder.Property(user => user.ConcurrencyStamp).HasMaxLength(64).IsConcurrencyToken();
            builder.HasIndex(user => user.DepartmentId);
            builder
                .HasOne<BasicDepartment>()
                .WithMany()
                .HasForeignKey(user => user.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<BasicRole>(builder =>
        {
            builder.ToTable(BasicDbProperties.TablePrefix + "Roles", BasicDbProperties.Schema);
            builder.HasKey(role => role.Id);
            builder.Property(role => role.Name).HasMaxLength(64).IsRequired();
            builder.HasIndex(role => role.Name).IsUnique();
            builder.Property(role => role.DisplayName).HasMaxLength(128).IsRequired();
            builder.Property(role => role.ConcurrencyStamp).HasMaxLength(64).IsConcurrencyToken();
        });

        modelBuilder.Entity<BasicMenu>(builder =>
        {
            builder.ToTable(BasicDbProperties.TablePrefix + "Menus", BasicDbProperties.Schema);
            builder.HasKey(menu => menu.Id);
            builder.Property(menu => menu.Name).HasMaxLength(128).IsRequired();
            builder.HasIndex(menu => menu.Name).IsUnique();
            builder.Property(menu => menu.DisplayName).HasMaxLength(128).IsRequired();
            builder.Property(menu => menu.Route).HasMaxLength(256);
            builder.Property(menu => menu.Component).HasMaxLength(256);
            builder.Property(menu => menu.Icon).HasMaxLength(128);
            builder.Property(menu => menu.PermissionName).HasMaxLength(256);
            builder.Property(menu => menu.ConcurrencyStamp).HasMaxLength(64).IsConcurrencyToken();
            builder.HasIndex(menu => new { menu.ParentId, menu.Sort });
            builder
                .HasOne<BasicMenu>()
                .WithMany()
                .HasForeignKey(menu => menu.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BasicDepartment>(builder =>
        {
            builder.ToTable(
                BasicDbProperties.TablePrefix + "Departments",
                BasicDbProperties.Schema
            );
            builder.HasKey(department => department.Id);
            builder.Property(department => department.Name).HasMaxLength(64).IsRequired();
            builder.HasIndex(department => department.Name).IsUnique();
            builder.Property(department => department.DisplayName).HasMaxLength(128).IsRequired();
            builder
                .Property(department => department.ConcurrencyStamp)
                .HasMaxLength(64)
                .IsConcurrencyToken();
            builder.HasIndex(department => new { department.ParentId, department.Sort });
            builder
                .HasOne<BasicDepartment>()
                .WithMany()
                .HasForeignKey(department => department.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BasicPermission>(builder =>
        {
            builder.ToTable(
                BasicDbProperties.TablePrefix + "Permissions",
                BasicDbProperties.Schema
            );
            builder.HasKey(permission => permission.Id);
            builder.Property(permission => permission.Name).HasMaxLength(256).IsRequired();
            builder.HasAlternateKey(permission => permission.Name);
            builder.Property(permission => permission.DisplayName).HasMaxLength(256).IsRequired();
            builder.Property(permission => permission.ParentName).HasMaxLength(256);
            builder
                .Property(permission => permission.ConcurrencyStamp)
                .HasMaxLength(64)
                .IsConcurrencyToken();
            builder.HasData(CreatePermissionSeedData());
        });

        modelBuilder.Entity<BasicPermissionGrant>(builder =>
        {
            builder.ToTable(
                BasicDbProperties.TablePrefix + "PermissionGrants",
                BasicDbProperties.Schema
            );
            builder.HasKey(grant => grant.Id);
            builder.Property(grant => grant.PermissionName).HasMaxLength(256).IsRequired();
            builder.Property(grant => grant.ProviderKey).HasMaxLength(64).IsRequired();
            builder
                .HasIndex(grant => new
                {
                    grant.PermissionName,
                    grant.Provider,
                    grant.ProviderKey,
                })
                .IsUnique();
            builder
                .HasOne<BasicPermission>()
                .WithMany()
                .HasForeignKey(grant => grant.PermissionName)
                .HasPrincipalKey(permission => permission.Name)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BasicUserRole>(builder =>
        {
            builder.ToTable(BasicDbProperties.TablePrefix + "UserRoles", BasicDbProperties.Schema);
            builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
            builder
                .HasOne<BasicUser>()
                .WithMany()
                .HasForeignKey(userRole => userRole.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne<BasicRole>()
                .WithMany()
                .HasForeignKey(userRole => userRole.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BasicRoleMenu>(builder =>
        {
            builder.ToTable(BasicDbProperties.TablePrefix + "RoleMenus", BasicDbProperties.Schema);
            builder.HasKey(roleMenu => new { roleMenu.RoleId, roleMenu.MenuId });
            builder
                .HasOne<BasicRole>()
                .WithMany()
                .HasForeignKey(roleMenu => roleMenu.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne<BasicMenu>()
                .WithMany()
                .HasForeignKey(roleMenu => roleMenu.MenuId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BasicRoleDepartment>(builder =>
        {
            builder.ToTable(
                BasicDbProperties.TablePrefix + "RoleDepartments",
                BasicDbProperties.Schema
            );
            builder.HasKey(item => new { item.RoleId, item.DepartmentId });
            builder
                .HasOne<BasicRole>()
                .WithMany()
                .HasForeignKey(item => item.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne<BasicDepartment>()
                .WithMany()
                .HasForeignKey(item => item.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        return modelBuilder;
    }

    private static IEnumerable<object> CreatePermissionSeedData()
    {
        return BasicPermissionNames.All.Select(
            (name, index) =>
            {
                var separatorIndex = name.LastIndexOf('.');
                return new
                {
                    Id = new Guid(index + 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                    Name = name,
                    DisplayName = name,
                    ParentName = separatorIndex < 0 ? null : name[..separatorIndex],
                    IsEnabled = true,
                    ConcurrencyStamp = $"basic-permission-{index + 1}",
                };
            }
        );
    }
}
