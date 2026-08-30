using Domain.Entities.Padron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Padron;

public class MesaOrdenConfiguration : IEntityTypeConfiguration<MesaOrden>
{
    public void Configure(EntityTypeBuilder<MesaOrden> builder)
    {
        builder.ToTable("mesa_orden", t => t.ExcludeFromMigrations());
        builder.HasKey(m => m.Cedula);
    }
}
