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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    public int? UbicacionId { get; set; }
    public int? OperadorUbicacionId { get; set; }

    public User? User { get; set; }
    public Elector Elector { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    //Guarda la ubicación del elector
    public Ubicacion? Ubicacion { get; set; }
    //Guarda la ubicación del operador durante la captación (Dato de auditoría)
    public Ubicacion? OperadorUbicacion { get; set; }
}
