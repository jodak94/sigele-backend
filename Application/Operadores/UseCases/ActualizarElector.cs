using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Domain.Entities;

namespace Application.Operadores.UseCases;

public class ActualizarElector
{
    private readonly IOperadorPersonaRepository _repository;
    private readonly IUbicacionRepository _ubicacionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ActualizarElector(
        IOperadorPersonaRepository repository,
        IUbicacionRepository ubicacionRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _ubicacionRepository = ubicacionRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int cedula, ActualizarElectorDto dto, CancellationToken cancellationToken = default)
    {
        var operadorId = _currentUserService.UserId;

        var registro = await _repository.GetAsync(operadorId, cedula, cancellationToken);
        if (registro is null || !registro.IsActive)
            throw new KeyNotFoundException("Asignación no encontrada.");

        registro.DisponibleMiembroMesa = dto.DisponibleMiembroMesa;
        registro.RequiereTransporte    = dto.RequiereTransporte;
        registro.NroTelefono           = dto.NroTelefono;
        registro.DireccionRecogida     = dto.DireccionRecogida;

        if (dto.Ubicacion is null)
        {
            registro.Ubicacion   = null;
            registro.UbicacionId = null;
        }
        else if (registro.Ubicacion is not null)
        {
            registro.Ubicacion.Lat         = dto.Ubicacion.Lat;
            registro.Ubicacion.Lng         = dto.Ubicacion.Lng;
            registro.Ubicacion.Descripcion = dto.Ubicacion.Descripcion;
        }
        else
        {
            var nuevaUbicacion = new Ubicacion
            {
                Lat         = dto.Ubicacion.Lat,
                Lng         = dto.Ubicacion.Lng,
                Descripcion = dto.Ubicacion.Descripcion,
                TenantId    = _currentUserService.TenantId,
                CreatedBy   = operadorId
            };
            await _ubicacionRepository.AddAsync(nuevaUbicacion, cancellationToken);
            registro.Ubicacion = nuevaUbicacion;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
