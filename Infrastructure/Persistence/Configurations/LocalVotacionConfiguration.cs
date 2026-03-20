using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class LocalVotacionConfiguration : IEntityTypeConfiguration<LocalVotacion>
{
    public void Configure(EntityTypeBuilder<LocalVotacion> builder)
    {
        builder.HasKey(l => l.SeccLoc);
        builder.Property(l => l.SeccLoc).ValueGeneratedNever();

        builder.Property(l => l.NombreLoc).HasMaxLength(80);
        builder.Property(l => l.Direccion).HasMaxLength(80);
        builder.Property(l => l.Recibido).HasMaxLength(1).IsFixedLength();
    }
}
