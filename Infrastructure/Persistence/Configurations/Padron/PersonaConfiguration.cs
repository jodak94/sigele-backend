using Domain.Entities.Padron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Padron;

public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
{
    public void Configure(EntityTypeBuilder<Persona> builder)
    {
        builder.ToTable("persona", t => t.ExcludeFromMigrations());
        builder.HasKey(p => p.Cedula);
        builder.Property(p => p.Nombre).HasMaxLength(35);
        builder.Property(p => p.Apellido).HasMaxLength(35);
        builder.Property(p => p.Sexo).HasMaxLength(1);
        builder.Property(p => p.Origen).IsRequired().HasMaxLength(30);

        builder.HasIndex(p => p.Apellido).HasDatabaseName("idx_persona_apellido");
        builder.HasIndex(p => p.Nombre).HasDatabaseName("idx_persona_nombre");
    }
}
