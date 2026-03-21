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
    public SeccionalDto? Seccional { get; set; }
}

public class LocalDto
{
    public int SeccLoc { get; set; }
    public string? NombreLoc { get; set; }
    public string? Direccion { get; set; }
}

public class SeccionalDto
{
    public string? NDepart { get; set; }
    public string? NDistrito { get; set; }
    public string? Descripcio { get; set; }
    public string? Direccion { get; set; }
}
