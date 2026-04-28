using Application.Electores.DTOs;

namespace Application.Common.Interfaces;

public interface IElectorPadronRepository
{
    Task<IEnumerable<ElectorDetailDto>> GetByNumeroCedAsync(int numeroCed, bool includeId, CancellationToken cancellationToken = default);
}
