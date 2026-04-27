using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class VehiculoConfiguration : IEntityTypeConfiguration<Vehiculo>
{
    public void Configure(EntityTypeBuilder<Vehiculo> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).ValueGeneratedOnAdd();
        builder.Property(v => v.NombreDueno).IsRequired().HasMaxLength(256);
        builder.Property(v => v.TelefonoDueno).IsRequired().HasMaxLength(50);
        builder.Property(v => v.MontoAlquiler).HasColumnType("numeric(10,2)");
        builder.Property(v => v.Observacion).HasMaxLength(500);
        builder.Property(v => v.CreatetAt).HasDefaultValueSql("NOW()");
        builder.Property(v => v.IsActive).HasDefaultValue(true);
        builder.HasOne(v => v.Operador)
            .WithMany()
            .HasForeignKey(v => v.OperadorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(v => v.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
