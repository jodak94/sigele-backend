using Domain.Entities;

namespace Application.Electores.Interfaces;

public interface IElectorRepository
{
    Task<IEnumerable<(Elector Elector, Seccional? Seccional)>> GetByNumeroCedAsync(int numeroCed, CancellationToken cancellationToken = default);
}
