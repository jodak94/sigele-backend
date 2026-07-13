using Domain.Entities.Padron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Padron;

public class RcpInscripcionConfiguration : IEntityTypeConfiguration<RcpInscripcion>
{
    public void Configure(EntityTypeBuilder<RcpInscripcion> builder)
    {
        builder.ToTable("inscripcion", t => t.ExcludeFromMigrations());
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();
        builder.Property(i => i.Tipo).HasMaxLength(1);
        builder.Property(i => i.Menores).HasMaxLength(1);
        builder.Property(i => i.Interdicto).HasMaxLength(1);
        builder.Property(i => i.PolMil).HasMaxLength(1);

        builder.HasOne(i => i.Persona)
            .WithMany(p => p.Inscripciones)
            .HasForeignKey(i => i.Cedula)
            .HasPrincipalKey(p => p.Cedula)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Localidad)
            .WithMany()
            .HasForeignKey(i => new { i.Depart, i.Distrito, i.Zona, i.Local })
            .HasPrincipalKey(l => new { l.Depart, l.Distrito, l.Zona, l.Local })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.ZonaNav)
            .WithMany()
            .HasForeignKey(i => new { i.Depart, i.Distrito, i.Zona })
            .HasPrincipalKey(z => new { z.Depart, z.Distrito, z.Zona })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
