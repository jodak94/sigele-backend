namespace Application.Asistencia.DTOs;

public record AsistenciaElectorDto(
    int UserId,
    int Cedula,
    string NombreApellido,
    string Telefono,
    string? LocalVotacion,
    string? OperadorResponsable,
    bool Asistio,
    DateTimeOffset? AsistioMarcadoEn
);

public record AsistenciaResumenDto(
    int TotalElectores,
    int TotalAsistieron,
    int TotalFaltantes,
    double PorcentajeAsistencia
);

public record MarcarAsistenciaDto(bool Asistio);
