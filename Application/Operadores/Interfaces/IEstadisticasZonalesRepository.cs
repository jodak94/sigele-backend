using Application.Operadores.DTOs;

namespace Application.Operadores.Interfaces;

public interface IEstadisticasZonalesRepository
{
    Task<IEnumerable<ZonaCaptacionDto>> GetZonasCaptacionAsync(int tenantId, int? coordinatorId, CancellationToken cancellationToken = default);
}
