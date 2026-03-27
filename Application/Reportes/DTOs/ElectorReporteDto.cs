namespace Application.Reportes.DTOs;

public record ElectorReporteDto(
    int     NroDocumento,
    string  Nombre,
    string  Apellido,
    string? LocalVotacion,
    short?  Mesa,
    short?  Orden
);

public record ReporteElectoresResult(
    string OperadorNombre,
    IEnumerable<ElectorReporteDto> Electores
);
