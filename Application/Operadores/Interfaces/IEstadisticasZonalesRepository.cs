using Application.Operadores.DTOs;

namespace Application.Operadores.Interfaces;

public interface IEstadisticasZonalesRepository
{
    Task<IEnumerable<SeccionalCaptacionDto>> GetSecccionalesCaptacionAsync(int tenantId, int? coordinatorId, CancellationToken cancellationToken = default);
}
