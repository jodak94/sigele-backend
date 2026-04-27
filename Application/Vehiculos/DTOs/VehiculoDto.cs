namespace Application.Vehiculos.DTOs;

public record VehiculoDto(
    int Id,
    int Capacidad,
    string NombreDueno,
    string TelefonoDueno,
    int? OperadorId,
    string? OperadorNombre,
    decimal MontoAlquiler,
    string? Observacion
);
