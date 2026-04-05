namespace Domain.Entities;

public class TenantPackage
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string PackageType { get; set; } = string.Empty;
    public int? ElectoresAgregados { get; set; }
    public decimal Precio { get; set; }
    public string CicloElectoral { get; set; } = string.Empty;
    public DateTime PurchasedAt { get; set; }
    public string? Notes { get; set; }

    public Tenant Tenant { get; set; } = null!;
}
