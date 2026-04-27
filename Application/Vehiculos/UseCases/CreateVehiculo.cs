using Application.Common.Interfaces;
using Application.Vehiculos.DTOs;
using Application.Vehiculos.Interfaces;
using Domain.Entities;

namespace Application.Vehiculos.UseCases;

public class CreateVehiculo
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantService _tenantService;
    private readonly ICurrentUserService _currentUserService;

    public CreateVehiculo(IVehiculoRepository vehiculoRepository, IUnitOfWork unitOfWork,
        ITenantService tenantService, ICurrentUserService currentUserService)
    {
        _vehiculoRepository = vehiculoRepository;
        _unitOfWork = unitOfWork;
        _tenantService = tenantService;
        _currentUserService = currentUserService;
    }

    public async Task<VehiculoDto> ExecuteAsync(CreateVehiculoDto dto, CancellationToken cancellationToken = default)
    {
        var vehiculo = new Vehiculo
        {
            Capacidad = dto.Capacidad,
            NombreDueno = dto.NombreDueno,
            TelefonoDueno = dto.TelefonoDueno,
            OperadorId = dto.OperadorId,
            MontoAlquiler = dto.MontoAlquiler,
            Observacion = dto.Observacion,
            TenantId = _tenantService.GetCurrentTenantId(),
            CreatedBy = _currentUserService.UserId
        };

        await _vehiculoRepository.AddAsync(vehiculo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new VehiculoDto(
            vehiculo.Id,
            vehiculo.Capacidad,
            vehiculo.NombreDueno,
            vehiculo.TelefonoDueno,
            vehiculo.OperadorId,
            null,
            vehiculo.MontoAlquiler,
            vehiculo.Observacion
        );
    }
}
