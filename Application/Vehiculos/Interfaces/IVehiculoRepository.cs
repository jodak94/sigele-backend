using Domain.Entities;

namespace Application.Vehiculos.Interfaces;

public interface IVehiculoRepository
{
    Task<Vehiculo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Vehiculo> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehiculo>> GetAllForReporteAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Vehiculo vehiculo, CancellationToken cancellationToken = default);
    void Update(Vehiculo vehiculo);
}
