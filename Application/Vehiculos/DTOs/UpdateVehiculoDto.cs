namespace Application.Vehiculos.DTOs;

public record UpdateVehiculoDto(
    int Capacidad,
    string NombreDueno,
    string TelefonoDueno,
    int? OperadorId,
    decimal MontoAlquiler,
    string? Observacion
);
