namespace Application.Vehiculos.DTOs;

public record CreateVehiculoDto(
    int Capacidad,
    string NombreDueno,
    string TelefonoDueno,
    int? OperadorId,
    decimal MontoAlquiler,
    string? Observacion
);
