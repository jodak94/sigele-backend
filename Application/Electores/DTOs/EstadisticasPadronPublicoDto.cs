namespace Application.Electores.DTOs;

public record EstadisticasPadronPublicoDto(
    long TotalConsultas,
    long UltimosSieteDias,
    long CedulasUnicas,
    int? HorarioPico   // hora del día 0-23 con más consultas, null si no hay datos
);
