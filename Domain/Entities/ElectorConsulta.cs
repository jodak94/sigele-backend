namespace Domain.Entities;

public class ElectorConsulta
{
    public long Id { get; set; }
    public int TenantId { get; set; }
    public string Cedula { get; set; } = string.Empty;

    public string IpCliente { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public string? Origin { get; set; }
    public string Host { get; set; } = string.Empty;
    public string MetodoHttp { get; set; } = "GET";

    public bool Encontrado { get; set; }
    public DateTimeOffset ConsultadoEn { get; set; } = DateTimeOffset.UtcNow;

    public Tenant Tenant { get; set; } = null!;
}
