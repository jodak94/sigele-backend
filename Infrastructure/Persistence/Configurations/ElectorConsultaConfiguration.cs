using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ElectorConsultaConfiguration : IEntityTypeConfiguration<ElectorConsulta>
{
    public void Configure(EntityTypeBuilder<ElectorConsulta> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).UseIdentityByDefaultColumn();
        builder.Property(e => e.Cedula).IsRequired().HasMaxLength(20);
        builder.Property(e => e.IpCliente).IsRequired().HasMaxLength(45);
        builder.Property(e => e.UserAgent).HasMaxLength(500);
        builder.Property(e => e.Origin).HasMaxLength(500);
        builder.Property(e => e.Host).IsRequired().HasMaxLength(253);
        builder.Property(e => e.MetodoHttp).IsRequired().HasMaxLength(10).HasDefaultValue("GET");
        builder.Property(e => e.ConsultadoEn).IsRequired().HasDefaultValueSql("now()");

        builder.HasOne(e => e.Tenant)
            .WithMany()
            .HasForeignKey(e => e.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.TenantId, e.ConsultadoEn });
        builder.HasIndex(e => e.Cedula);
        builder.HasIndex(e => e.IpCliente);
    }
}
