using Domain.Common;

namespace Domain.Entities;

public class Ubicacion : AuditableEntity
{
    public int Id { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}
