using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.Property(p => p.Description)
            .HasMaxLength(250);

        builder.HasData(
            new Permission { Id = 1, Name = "user:read", Description = "Can read users" },
            new Permission { Id = 2, Name = "user:create-operator", Description = "Can create operator users" },
            new Permission { Id = 3, Name = "user:create-coordinator", Description = "Can create coordinator users" }
        );
    }
}