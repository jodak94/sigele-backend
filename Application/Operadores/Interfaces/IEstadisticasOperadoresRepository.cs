using Application.Operadores.DTOs;

namespace Application.Operadores.Interfaces;

public interface IEstadisticasOperadoresRepository
{
    Task<EstadisticasOperadoresDto> GetEstadisticasAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default);
}
