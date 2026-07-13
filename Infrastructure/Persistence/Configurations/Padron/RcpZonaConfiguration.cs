using Domain.Entities.Padron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Padron;

public class RcpZonaConfiguration : IEntityTypeConfiguration<RcpZona>
{
    public void Configure(EntityTypeBuilder<RcpZona> builder)
    {
        builder.ToTable("zona", t => t.ExcludeFromMigrations());
        builder.HasKey(z => new { z.Depart, z.Distrito, z.Zona });
        builder.Property(z => z.Descrip).HasMaxLength(60);
    }
}
