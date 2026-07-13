using Domain.Entities.Padron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Padron;

public class RcpDistritoConfiguration : IEntityTypeConfiguration<RcpDistrito>
{
    public void Configure(EntityTypeBuilder<RcpDistrito> builder)
    {
        builder.ToTable("distrito", t => t.ExcludeFromMigrations());
        builder.HasKey(d => new { d.Depart, d.Distrito });
        builder.Property(d => d.Descrip).HasMaxLength(50);
    }
}
