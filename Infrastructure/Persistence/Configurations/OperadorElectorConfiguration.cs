using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OperadorElectorConfiguration : IEntityTypeConfiguration<OperadorElector>
{
    public void Configure(EntityTypeBuilder<OperadorElector> builder)
    {
        builder.HasKey(oe => new { oe.UserId, oe.ElectorId });

        builder.Property(oe => oe.NroTelefono).IsRequired().HasMaxLength(20);
        builder.Property(oe => oe.DireccionRecogida).HasMaxLength(255);
        builder.Property(oe => oe.IsActive).HasDefaultValue(true);

        builder.HasOne(oe => oe.User)
            .WithMany()
            .HasForeignKey(oe => oe.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oe => oe.Elector)
            .WithMany()
            .HasForeignKey(oe => oe.ElectorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oe => oe.Tenant)
            .WithMany()
            .HasForeignKey(oe => oe.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(oe => oe.UserId);

        // Un elector solo puede estar activo en un operador por tenant
        builder.HasIndex(oe => new { oe.ElectorId, oe.TenantId })
            .IsUnique()
            .HasFilter("is_active = true");
    }
}
