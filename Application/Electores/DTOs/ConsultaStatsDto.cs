namespace Application.Electores.DTOs;

// Dapper necesita { get; set; } — no usar positional record
public class TopLocalConsultadoDto
{
    public string? LocalVotacion  { get; set; }
    public long    TotalBusquedas { get; set; }
}

public record UltimaConsultaDto(
    DateTimeOffset FechaHora,
    string         Cedula,
    bool           Encontrado
);
