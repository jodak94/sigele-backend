namespace Application.Reportes.DTOs;

public record VehiculoReporteItemDto(
    string NombreDueno,
    string TelefonoDueno,
    int Capacidad,
    decimal MontoAlquiler,
    string? OperadorNombre,
    string? Observacion
);

public record VehiculosReporteData(
    IEnumerable<VehiculoReporteItemDto> Vehiculos,
    int Total
);
