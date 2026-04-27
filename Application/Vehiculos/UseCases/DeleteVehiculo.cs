using Application.Common.Interfaces;
using Application.Vehiculos.Interfaces;

namespace Application.Vehiculos.UseCases;

public class DeleteVehiculo
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteVehiculo(IVehiculoRepository vehiculoRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _vehiculoRepository = vehiculoRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var vehiculo = await _vehiculoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehículo con id {id} no encontrado.");

        vehiculo.IsActive = false;
        vehiculo.DeletedAt = DateTime.UtcNow;
        vehiculo.DeletedBy = _currentUserService.UserId;

        _vehiculoRepository.Update(vehiculo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
