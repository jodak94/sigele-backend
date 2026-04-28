using Domain.Common;

namespace Domain.Entities;

public enum EstadoSolicitudVehiculo
{
    Pendiente = 0,
    Aprobada = 1,
    Rechazada = 2
}

public class VehiculoRequest : AuditableEntity
{
    public int Id { get; set; }
    public int ElectorId { get; set; }
    public Elector Elector { get; set; } = null!;
    public int OperadorId { get; set; }
    public User Operador { get; set; } = null!;
    public string NombreDueno { get; set; } = string.Empty;
    public string TelefonoDueno { get; set; } = string.Empty;
    public int Capacidad { get; set; }
    public decimal? MontoAlquiler { get; set; }
    public EstadoSolicitudVehiculo Estado { get; set; } = EstadoSolicitudVehiculo.Pendiente;
    public string? Observacion { get; set; }
    public int? AprobadoPorId { get; set; }
    public User? AprobadoPor { get; set; }
    public DateTime? FechaResolucion { get; set; }
}
