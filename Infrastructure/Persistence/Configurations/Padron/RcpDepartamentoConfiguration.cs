using Domain.Entities.Padron;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Padron;

public class RcpDepartamentoConfiguration : IEntityTypeConfiguration<RcpDepartamento>
{
    public void Configure(EntityTypeBuilder<RcpDepartamento> builder)
    {
        builder.ToTable("departamento", t => t.ExcludeFromMigrations());
        builder.HasKey(d => d.Depart);
        builder.Property(d => d.Descrip).HasMaxLength(15);
    }
}
