using Domain.Common;

namespace Domain.Entities;

public class Vehiculo : AuditableEntity
{
    public int Id { get; set; }
    public int Capacidad { get; set; }
    public string NombreDueno { get; set; } = string.Empty;
    public string TelefonoDueno { get; set; } = string.Empty;
    public int? OperadorId { get; set; }
    public User? Operador { get; set; }
    public decimal MontoAlquiler { get; set; }
    public string? Observacion { get; set; }
}
