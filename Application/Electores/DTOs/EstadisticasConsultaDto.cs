namespace Application.Electores.DTOs;

public record EstadisticasConsultaDto(
    long Hoy,
    long Ayer,
    long UltimosSieteDias,
    long Total
);
