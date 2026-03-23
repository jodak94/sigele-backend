namespace Application.Reportes.DTOs;

// Resultado flat del repositorio — incluye el local para poder agrupar en el use case
public record DiaDFlatItemDto(
    string? LocalVotacion,
    short?  Mesa,
    short?  Orden,
    int     NroCedula,
    string  NombreApellido,
    string  Telefono,
    string? DireccionRecogida,
    bool    RequiereTransporte,
    string  OperadorResponsable
);

// Fila dentro de un grupo (ya sin LocalVotacion — es el header del grupo)
public record DiaDElectorItemDto(
    short?  Mesa,
    short?  Orden,
    int     NroCedula,
    string  NombreApellido,
    string  Telefono,
    string? DireccionRecogida,
    bool    RequiereTransporte,
    string  OperadorResponsable
);

public record DiaDLocalDto(
    string LocalVotacion,
    IEnumerable<DiaDElectorItemDto> Electores
);

public record DiaDResult(
    IEnumerable<DiaDLocalDto> Locales
);
