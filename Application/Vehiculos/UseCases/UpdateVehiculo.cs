using Application.Common.Interfaces;
using Application.Vehiculos.DTOs;
using Application.Vehiculos.Interfaces;

namespace Application.Vehiculos.UseCases;

public class UpdateVehiculo
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateVehiculo(IVehiculoRepository vehiculoRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _vehiculoRepository = vehiculoRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int id, UpdateVehiculoDto dto, CancellationToken cancellationToken = default)
    {
        var vehiculo = await _vehiculoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehículo con id {id} no encontrado.");

        vehiculo.Capacidad = dto.Capacidad;
        vehiculo.NombreDueno = dto.NombreDueno;
        vehiculo.TelefonoDueno = dto.TelefonoDueno;
        vehiculo.OperadorId = dto.OperadorId;
        vehiculo.MontoAlquiler = dto.MontoAlquiler;
        vehiculo.Observacion = dto.Observacion;
        vehiculo.UpdatedAt = DateTime.UtcNow;
        vehiculo.UpdatedBy = _currentUserService.UserId;

        _vehiculoRepository.Update(vehiculo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
