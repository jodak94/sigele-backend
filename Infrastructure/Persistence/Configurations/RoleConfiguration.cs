using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j =>
            {
                j.ToTable("PermissionRole");
                j.HasData(
                    // Admin gets all permissions
                    new { RolesId = 1, PermissionsId = 1 },
                    new { RolesId = 1, PermissionsId = 2 },
                    new { RolesId = 1, PermissionsId = 3 },
                    // Coordinator gets user:read and user:create-operator
                    new { RolesId = 2, PermissionsId = 1 },
                    new { RolesId = 2, PermissionsId = 2 },
                    // Operator gets only user:read
                    new { RolesId = 3, PermissionsId = 1 }
                );
            });

        builder.HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Coordinator" },
            new Role { Id = 3, Name = "Operator" }
        );
    }
}