namespace Application.Reportes.DTOs;

public record ResumenOperadorItemDto(
    string NombreOperador,
    string Telefono,
    int    ElectoresCaptados,
    int    MiembrosMesaDisp,
    int    ReqTransporte
);

public record ResumenOperadoresTotalesDto(
    int ElectoresCaptados,
    int MiembrosMesaDisp,
    int ReqTransporte
);

public record CoordinadorInfoDto(
    int    Id,
    string Nombre,
    string Telefono
);

public record ResumenOperadoresData(
    IEnumerable<ResumenOperadorItemDto> Operadores,
    ResumenOperadoresTotalesDto         Totales,
    CoordinadorInfoDto?                 Coordinador  // null cuando quien solicita es Admin
);
