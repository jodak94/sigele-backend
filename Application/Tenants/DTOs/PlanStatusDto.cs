namespace Application.Tenants.DTOs;

public class PlanStatusDto
{
    public int ElectorCount { get; set; }
    public int? ElectorLimit { get; set; }
    public bool EsPlanFull { get; set; }
    public bool EnGracia { get; set; }
    public bool CaptacionBloqueada { get; set; }
    public decimal PorcentajeUso { get; set; }
    public DateTime? AccessExpiresAt { get; set; }
    public bool AccesoVencido { get; set; }
    public List<PaqueteDto> Paquetes { get; set; } = [];
}

public class PaqueteDto
{
    public string PackageType { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int? ElectoresAgregados { get; set; }
    public decimal Precio { get; set; }
    public DateTime PurchasedAt { get; set; }
}
