namespace Application.Operadores.DTOs;

public record ActualizarElectorDto(
    bool DisponibleMiembroMesa,
    bool RequiereTransporte,
    string NroTelefono,
    string? DireccionRecogida,
    UbicacionInputDto? Ubicacion
);
