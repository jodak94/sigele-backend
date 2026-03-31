namespace Domain.Entities;

public class OperadorElector
{
    public int UserId { get; set; }
    public int ElectorId { get; set; }
    public int TenantId { get; set; }

    public bool DisponibleMiembroMesa { get; set; }
    public bool RequiereTransporte { get; set; }
    public string NroTelefono { get; set; } = string.Empty;
    public string? DireccionRecogida { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }

    public int? UbicacionId { get; set; }

    public User User { get; set; } = null!;
    public Elector Elector { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public Ubicacion? Ubicacion { get; set; }
}
