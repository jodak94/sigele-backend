using Application.Operadores.DTOs;
using Domain.Entities;

namespace Application.Operadores.Interfaces;

public interface IOperadorElectorRepository
{
    Task<bool> ElectorActivoEnTenantAsync(int electorId, int tenantId, CancellationToken cancellationToken = default);
    Task AddAsync(OperadorElector operadorElector, CancellationToken cancellationToken = default);
    Task<IEnumerable<OperadorElectorDto>> GetByOperadorAsync(int operadorId, CancellationToken cancellationToken = default);
    Task<OperadorElector?> GetAsync(int operadorId, int electorId, CancellationToken cancellationToken = default);
}
