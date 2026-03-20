using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ElectorConfiguration : IEntityTypeConfiguration<Elector>
{
    public void Configure(EntityTypeBuilder<Elector> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Apellido).HasMaxLength(30);
        builder.Property(e => e.Nombre).HasMaxLength(30);
        builder.Property(e => e.Direccion).HasMaxLength(60);
        builder.Property(e => e.KeyDD).HasMaxLength(4).IsFixedLength();
        builder.Property(e => e.CedApeNom).HasMaxLength(15);

        builder.HasOne(e => e.Local)
            .WithMany(l => l.Electores)
            .HasForeignKey(e => e.SecLoc)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.Apellido).HasDatabaseName("idx_electores_apellido");
        builder.HasIndex(e => e.Nombre).HasDatabaseName("idx_electores_nombre");
        builder.HasIndex(e => new { e.CodDpto, e.CodDist }).HasDatabaseName("idx_electores_dpto_dist");
        builder.HasIndex(e => new { e.CodigoSec, e.Mesa }).HasDatabaseName("idx_electores_sec_mesa");
    }
}
