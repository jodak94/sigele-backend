namespace Application.Auditoria.DTOs;

public record MapaOperadorDto(
    int    OperadorId,
    string OperadorNombre,
    string OperadorEmail,
    int    ElectorId,
    string? ElectorNombre,
    string? ElectorApellido,
    int    ElectorCedula,
    double Lat,
    double Lng,
    string Descripcion,
    DateTime CreadoEn
);
