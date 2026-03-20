using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SeccionalConfiguration : IEntityTypeConfiguration<Seccional>
{
    public void Configure(EntityTypeBuilder<Seccional> builder)
    {
        builder.HasKey(s => new { s.CodigoDep, s.CodigoDis, s.CodigoSec });

        builder.Property(s => s.NDepart).HasMaxLength(25);
        builder.Property(s => s.NDistrito).HasMaxLength(30);
        builder.Property(s => s.Descripcio).HasMaxLength(30);
        builder.Property(s => s.WSeccio).HasMaxLength(35);
        builder.Property(s => s.Direccion).HasMaxLength(45);
    }
}
