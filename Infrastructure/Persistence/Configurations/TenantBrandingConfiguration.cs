using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TenantBrandingConfiguration : IEntityTypeConfiguration<TenantBranding>
{
    public void Configure(EntityTypeBuilder<TenantBranding> builder)
    {
        builder.HasKey(tb => tb.Id);
        builder.Property(tb => tb.Id).ValueGeneratedOnAdd();

        builder.Property(tb => tb.AppTitle).IsRequired().HasMaxLength(150);
        builder.Property(tb => tb.PrimaryColor).IsRequired().HasMaxLength(20);
        builder.Property(tb => tb.SecondaryColor).HasMaxLength(20);
        builder.Property(tb => tb.FaviconUrl).HasMaxLength(500);
        builder.Property(tb => tb.CandidateName).HasMaxLength(150);
        builder.Property(tb => tb.CandidateTitle).HasMaxLength(150);
        builder.Property(tb => tb.Zona).HasMaxLength(150);

        builder.Property(tb => tb.IsActive).HasDefaultValue(true);
        builder.Property(tb => tb.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(tb => tb.UpdatedAt).HasDefaultValueSql("NOW()");

        builder.HasOne(tb => tb.Tenant)
            .WithMany()
            .HasForeignKey(tb => tb.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tb => tb.TenantId).IsUnique();
    }
}
