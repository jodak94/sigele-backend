using Application.Common.Interfaces;
using Application.VehiculoRequests.DTOs;
using Application.VehiculoRequests.Interfaces;
using Domain.Entities;

namespace Application.VehiculoRequests.UseCases;

public class RechazarVehiculoRequest
{
    private readonly IVehiculoRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RechazarVehiculoRequest(
        IVehiculoRequestRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int id, ResolveVehiculoRequestDto dto, CancellationToken cancellationToken = default)
    {
        var solicitud = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Solicitud de vehículo no encontrada.");

        if (solicitud.Estado != EstadoSolicitudVehiculo.Pendiente)
            throw new InvalidOperationException("La solicitud ya fue resuelta.");

        solicitud.Estado          = EstadoSolicitudVehiculo.Rechazada;
        solicitud.Observacion     = dto.Observacion;
        solicitud.AprobadoPorId   = _currentUserService.UserId;
        solicitud.FechaResolucion = DateTime.UtcNow;
        solicitud.UpdatedBy       = _currentUserService.UserId;
        solicitud.UpdatedAt       = DateTime.UtcNow;
        _repository.Update(solicitud);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
