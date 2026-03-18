using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    /*
     *
    public int Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
     * 
     */
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Subdomain).IsRequired().HasMaxLength(100);
        builder.HasIndex(t => t.Subdomain).IsUnique();
        builder.Property(t => t.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(t => t.IsActive).HasDefaultValue(true);
    }
    
}