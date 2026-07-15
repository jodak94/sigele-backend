using Domain.Entities.Padron;

namespace Domain.Entities;

public class OperadorPersona
{
    public int UserId { get; set; }
    public int Cedula { get; set; }
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

    public bool Asistio { get; set; }
    public DateTimeOffset? AsistioMarcadoEn { get; set; }
    public int? AsistioMarcadoPor { get; set; }

    public User? User { get; set; }
    public Persona Persona { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public Ubicacion? Ubicacion { get; set; }
    public Ubicacion? OperadorUbicacion { get; set; }
    public User? AsistioMarcadoPorUser { get; set; }
}
