using System.Text.Json.Serialization;

namespace Application.Electores.DTOs;

public class ElectorDetailDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Id { get; set; }
    public int NumeroCed { get; set; }
    public string? Apellido { get; set; }
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public DateOnly? FechaNaci { get; set; }
    public short? Mesa { get; set; }
    public short? Orden { get; set; }
    public short? CodigoSex { get; set; }
    public LocalDto? Local { get; set; }
    public ZonaDto? Zona { get; set; }
}

public class LocalDto
{
    public int CodigoLocal { get; set; }
    public string? NombreLoc { get; set; }
    public string? Direccion { get; set; }
}

public class ZonaDto
{
    public short? Depart { get; set; }
    public short? Distrito { get; set; }
    public short? Zona { get; set; }
    public string? NombreDepart { get; set; }
    public string? NombreDistrito { get; set; }
    public string? NombreZona { get; set; }
}
