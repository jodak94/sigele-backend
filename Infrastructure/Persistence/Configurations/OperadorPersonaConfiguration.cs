using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OperadorPersonaConfiguration : IEntityTypeConfiguration<OperadorPersona>
{
    public void Configure(EntityTypeBuilder<OperadorPersona> builder)
    {
        builder.HasKey(op => new { op.UserId, op.Cedula });

        builder.Property(op => op.NroTelefono).IsRequired().HasMaxLength(20);
        builder.Property(op => op.DireccionRecogida).HasMaxLength(255);
        builder.Property(op => op.IsActive).HasDefaultValue(true);
        builder.Property(op => op.CreatedAt).HasDefaultValueSql("NOW()");

        builder.HasOne(op => op.User)
            .WithMany()
            .HasForeignKey(op => op.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(op => op.Persona)
            .WithMany()
            .HasForeignKey(op => op.Cedula)
            .HasPrincipalKey(p => p.Cedula)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(op => op.Tenant)
            .WithMany()
            .HasForeignKey(op => op.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(op => op.Ubicacion)
            .WithMany()
            .HasForeignKey(op => op.UbicacionId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne(op => op.OperadorUbicacion)
            .WithMany()
            .HasForeignKey(op => op.OperadorUbicacionId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasIndex(op => op.UserId);

        builder.HasIndex(op => new { op.Cedula, op.TenantId })
            .IsUnique()
            .HasFilter("is_active = true");
    }
}
