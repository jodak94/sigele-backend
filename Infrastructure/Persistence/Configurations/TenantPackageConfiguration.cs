using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TenantPackageConfiguration : IEntityTypeConfiguration<TenantPackage>
{
    public void Configure(EntityTypeBuilder<TenantPackage> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.PackageType).IsRequired().HasMaxLength(50);
        builder.Property(p => p.ElectoresAgregados);
        builder.Property(p => p.Precio).HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(p => p.CicloElectoral).IsRequired().HasMaxLength(50);
        builder.Property(p => p.PurchasedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.Notes).HasMaxLength(500).IsRequired(false);

        builder.HasOne(p => p.Tenant)
            .WithMany()
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
