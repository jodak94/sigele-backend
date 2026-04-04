namespace Application.Auditoria.DTOs;

public record CaptacionDetalleDto(
    string? ElectorNombre,
    string? ElectorApellido,
    int     CedulaElector,
    DateTime CreatedAt
);
