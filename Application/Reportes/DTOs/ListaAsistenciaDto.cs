namespace Application.Reportes.DTOs;

public record ListaAsistenciaItemDto(
    int    NroCedula,
    string NombreApellido,
    string Telefono,
    string? LocalVotacion,
    short?  Mesa,
    short?  Orden
);

public record ListaAsistenciaResult(
    string OperadorNombre,
    IEnumerable<ListaAsistenciaItemDto> Items
);
