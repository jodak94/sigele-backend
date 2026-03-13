using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(256);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Password).IsRequired().HasMaxLength(256);
        builder.Property(u => u.Phone).IsRequired().HasMaxLength(50);
        builder.Property(u => u.CreatetAt).HasDefaultValue(DateTime.UtcNow);
        builder.Property(u => u.UpdatedAt).ValueGeneratedOnUpdate();
        builder.Property(u => u.IsActive).HasDefaultValue(true);
    }
}