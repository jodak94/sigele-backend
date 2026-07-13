namespace Application.Operadores.DTOs;

public class ZonaCaptacionDto
{
    public short?  Depart         { get; set; }
    public short?  Distrito       { get; set; }
    public short?  Zona           { get; set; }
    public string? Descripcion    { get; set; }
    public int     TotalElectores { get; set; }
}

public record ResumenZonalCaptacionDto(
    ZonaCaptacionDto? ZonaMayorCaptacion,
    ZonaCaptacionDto? ZonaMenorCaptacion,
    double             PromedioPorZona);
