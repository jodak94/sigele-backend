namespace Application.Operadores.DTOs;

public class SeccionalCaptacionDto
{
    public short? CodigoSeccional { get; set; }
    public int    TotalElectores  { get; set; }
}

public record ResumenZonalCaptacionDto(
    SeccionalCaptacionDto? ZonaMayorCaptacion,
    SeccionalCaptacionDto? ZonaMenorCaptacion,
    double                 PromedioPorSeccional);
