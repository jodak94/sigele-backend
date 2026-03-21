namespace Application.Operadores.DTOs;

public record AsignarElectorDto(
    int ElectorId,
    bool DisponibleMiembroMesa,
    bool RequiereTransporte,
    string NroTelefono,
    string? DireccionRecogida
);
