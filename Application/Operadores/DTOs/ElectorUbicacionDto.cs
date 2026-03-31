namespace Application.Operadores.DTOs;

public record ElectorUbicacionDto(
    int ElectorId,
    string? Nombre,
    string? Apellido,
    int NumeroCed,
    double Lat,
    double Lng,
    string Descripcion
);
