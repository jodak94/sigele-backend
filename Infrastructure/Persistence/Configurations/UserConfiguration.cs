using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(256);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.HasIndex(u => new { u.Email, u.TenantId}).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
        builder.Property(u => u.Phone).IsRequired().HasMaxLength(50);
        builder.Property(u => u.CreatetAt).HasDefaultValueSql("NOW()");
        builder.Property(u => u.UpdatedAt).ValueGeneratedOnUpdate();
        builder.Property(u => u.IsActive).HasDefaultValue(true);
        builder.Property(u => u.MustChangePassword).HasDefaultValue(true);
        builder.HasOne(u => u.Coordinator)
            .WithMany(u => u.Operators)
            .HasForeignKey(u => u.CoordinatorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}