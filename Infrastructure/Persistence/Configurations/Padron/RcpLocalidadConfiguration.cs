using Domain.Entities.Padron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Padron;

public class RcpLocalidadConfiguration : IEntityTypeConfiguration<RcpLocalidad>
{
    public void Configure(EntityTypeBuilder<RcpLocalidad> builder)
    {
        builder.ToTable("localidad", t => t.ExcludeFromMigrations());
        builder.HasKey(l => new { l.Depart, l.Distrito, l.Zona, l.Local });
        builder.Property(l => l.Descrip).HasMaxLength(100);

        builder.HasOne(l => l.Dpto)
            .WithMany()
            .HasForeignKey(l => l.Depart)
            .HasPrincipalKey(d => d.Depart)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.DistNav)
            .WithMany()
            .HasForeignKey(l => new { l.Depart, l.Distrito })
            .HasPrincipalKey(d => new { d.Depart, d.Distrito })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
