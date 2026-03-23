namespace Application.Reportes.DTOs;

public record CandidatoMesaFlatItemDto(
    string? LocalVotacion,
    string  NombreApellido,
    int     NroCedula,
    string  Telefono,
    short?  Mesa,
    string  OperadorResponsable
);

public record CandidatoMesaItemDto(
    string  NombreApellido,
    int     NroCedula,
    string  Telefono,
    short?  Mesa,
    string  OperadorResponsable
);

public record CandidatoMesaLocalDto(
    string LocalVotacion,
    IEnumerable<CandidatoMesaItemDto> Candidatos
);

public record CandidatosMesaResult(
    IEnumerable<CandidatoMesaLocalDto> Locales
);
