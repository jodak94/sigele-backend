namespace Domain.Entities;

public class Elector
{
    public int Id { get; set; }
    public int NumeroCed { get; set; }
    public short? CodDpto { get; set; }
    public short? CodDist { get; set; }
    public short? SecAnt { get; set; }
    public short? CodigoSec { get; set; }
    public short? SLocal { get; set; }
    public short? Mesa { get; set; }
    public short? Orden { get; set; }
    public string? Apellido { get; set; }
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public DateOnly? FechaNaci { get; set; }
    public DateOnly? FechaAfil { get; set; }
    public short? Anio { get; set; }
    public short? CodigoSex { get; set; }
    public int? SecLoc { get; set; }
    public string? KeyDD { get; set; }
    public string? CedApeNom { get; set; }

    public LocalVotacion? Local { get; set; }
}
