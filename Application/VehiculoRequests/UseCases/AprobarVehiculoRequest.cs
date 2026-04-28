using Application.Common.Interfaces;
using Application.VehiculoRequests.DTOs;
using Application.VehiculoRequests.Interfaces;
using Application.Vehiculos.Interfaces;
using Domain.Entities;

namespace Application.VehiculoRequests.UseCases;

public class AprobarVehiculoRequest
{
    private readonly IVehiculoRequestRepository _requestRepository;
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AprobarVehiculoRequest(
        IVehiculoRequestRepository requestRepository,
        IVehiculoRepository vehiculoRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _requestRepository = requestRepository;
        _vehiculoRepository = vehiculoRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int id, ResolveVehiculoRequestDto dto, CancellationToken cancellationToken = default)
    {
        var solicitud = await _requestRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Solicitud de vehículo no encontrada.");

        if (solicitud.Estado != EstadoSolicitudVehiculo.Pendiente)
            throw new InvalidOperationException("La solicitud ya fue resuelta.");

        solicitud.Estado          = EstadoSolicitudVehiculo.Aprobada;
        solicitud.Observacion     = dto.Observacion;
        solicitud.AprobadoPorId   = _currentUserService.UserId;
        solicitud.FechaResolucion = DateTime.UtcNow;
        solicitud.UpdatedBy       = _currentUserService.UserId;
        solicitud.UpdatedAt       = DateTime.UtcNow;
        _requestRepository.Update(solicitud);

        var vehiculo = new Vehiculo
        {
            NombreDueno   = solicitud.NombreDueno,
            TelefonoDueno = solicitud.TelefonoDueno,
            Capacidad     = solicitud.Capacidad,
            MontoAlquiler = dto.MontoAlquiler ?? solicitud.MontoAlquiler ?? 0,
            //OperadorId    = solicitud.OperadorId,
            Observacion   = dto.Observacion,
            TenantId      = solicitud.TenantId,
            CreatedBy     = _currentUserService.UserId
        };

        await _vehiculoRepository.AddAsync(vehiculo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
