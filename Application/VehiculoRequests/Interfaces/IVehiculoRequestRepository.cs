using Domain.Entities;

namespace Application.VehiculoRequests.Interfaces;

public interface IVehiculoRequestRepository
{
    Task<VehiculoRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<VehiculoRequest> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(VehiculoRequest request, CancellationToken cancellationToken = default);
    void Update(VehiculoRequest request);
}
