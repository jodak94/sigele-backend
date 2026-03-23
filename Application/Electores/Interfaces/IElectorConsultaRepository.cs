using Application.Electores.DTOs;
using Domain.Entities;

namespace Application.Electores.Interfaces;

public interface IElectorConsultaRepository
{
    Task RegistrarAsync(ElectorConsulta consulta, CancellationToken cancellationToken = default);
    Task<EstadisticasConsultaDto> GetEstadisticasAsync(int tenantId, CancellationToken cancellationToken = default);
    Task<EstadisticasPadronPublicoDto> GetEstadisticasPadronPublicoAsync(int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TopLocalConsultadoDto>> GetTopLocalesConsultadosAsync(int tenantId, int top, CancellationToken cancellationToken = default);
    Task<IEnumerable<UltimaConsultaDto>> GetUltimasConsultasAsync(int tenantId, int top, CancellationToken cancellationToken = default);
}
