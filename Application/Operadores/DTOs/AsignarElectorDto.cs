namespace Application.Operadores.DTOs;

public record AsignarElectorDto(
    int ElectorId,
    bool DisponibleMiembroMesa,
    bool RequiereTransporte,
    string NroTelefono,
    string? DireccionRecogida,
    UbicacionInputDto? Ubicacion,
    UbicacionInputDto? OperadorUbicacion,
    bool SolicitudAlquiler = false,
    int? CapacidadVehiculo = null,
    decimal? MontoAlquilerVehiculo = null
);
