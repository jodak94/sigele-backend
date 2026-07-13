using Domain.Entities.Padron;

namespace Application.Padron;

public interface IPersonaRepository
{
    Task<Persona?> GetByCedulaAsync(int cedula, CancellationToken cancellationToken = default);
}
