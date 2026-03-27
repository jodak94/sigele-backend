namespace Application.Operadores.DTOs;

public record OperadorElectorDto(
    int ElectorId,
    string? Nombre,
    string? Apellido,
    int NumeroCed,
    bool DisponibleMiembroMesa,
    bool RequiereTransporte,
    string NroTelefono,
    string? DireccionRecogida,
    string? LocalVotacion,
    short? Mesa,
    short? Orden
);
