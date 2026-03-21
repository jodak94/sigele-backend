namespace Application.Reportes.DTOs;

public record ElectorReporteDto(
    string Nombre,
    string Apellido,
    int NroDocumento,
    string NroTelefono,
    bool MiembroMesa,
    bool RequiereTransporte,
    string? Direccion
);

public record ReporteElectoresResult(
    string OperadorNombre,
    IEnumerable<ElectorReporteDto> Electores
);
