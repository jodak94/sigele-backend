using Domain.Entities;

namespace Application.Operadores.Interfaces;

public interface IUbicacionRepository
{
    Task AddAsync(Ubicacion ubicacion, CancellationToken cancellationToken = default);
}
