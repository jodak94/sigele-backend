using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SeccLocalConfiguration : IEntityTypeConfiguration<SeccLocal>
{
    public void Configure(EntityTypeBuilder<SeccLocal> builder)
    {
        builder.HasKey(sl => new { sl.CodigoDep, sl.CodigoDis, sl.CodigoSec, sl.CodigoLoc });

        builder.HasOne(sl => sl.Seccional)
            .WithMany(s => s.SeccLocales)
            .HasForeignKey(sl => new { sl.CodigoDep, sl.CodigoDis, sl.CodigoSec })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sl => sl.Local)
            .WithMany(l => l.SeccLocales)
            .HasForeignKey(sl => sl.SeccLoc)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sl => sl.SeccLoc).HasDatabaseName("idx_secc_locales_secc_loc");
    }
}
