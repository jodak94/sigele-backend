namespace Application.VehiculoRequests.DTOs;

public record VehiculoRequestDto(
    int Id,
    int ElectorId,
    int OperadorId,
    string OperadorNombre,
    string NombreDueno,
    string TelefonoDueno,
    int Capacidad,
    decimal? MontoAlquiler,
    string Estado,
    string? Observacion,
    string? AprobadoPorNombre,
    DateTime? FechaResolucion,
    DateTime CreadoEn
);
