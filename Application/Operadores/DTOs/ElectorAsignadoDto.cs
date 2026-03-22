namespace Application.Operadores.DTOs;

public record OperadorBasicoDto(int Id, string FullName, string Email, string Phone);

public record ElectorAsignadoDto(
    int ElectorId,
    string? Nombre,
    string? Apellido,
    int NumeroCed,
    bool DisponibleMiembroMesa,
    bool RequiereTransporte,
    string NroTelefono,
    string? DireccionRecogida,
    OperadorBasicoDto Operador
);
