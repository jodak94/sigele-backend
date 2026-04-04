using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Application.Users.Interfaces;
using Domain.Entities;

namespace Application.Operadores.UseCases;

public class AsignarElector
{
    private readonly IOperadorElectorRepository _operadorElectorRepository;
    private readonly IUbicacionRepository _ubicacionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AsignarElector(
        IOperadorElectorRepository operadorElectorRepository,
        IUbicacionRepository ubicacionRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _ubicacionRepository = ubicacionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int operadorId, AsignarElectorDto dto, CancellationToken cancellationToken = default)
    {
        var tenantId = _currentUserService.TenantId;

        var operador = await _userRepository.GetByIdAsync(operadorId, cancellationToken);
        if (operador is null || operador.TenantId != tenantId)
            throw new KeyNotFoundException("Operador no encontrado.");

        var yaAsignado = await _operadorElectorRepository.ElectorActivoEnTenantAsync(dto.ElectorId, tenantId, cancellationToken);
        if (yaAsignado)
            throw new InvalidOperationException("El elector ya está asignado a otro operador en este tenant.");

        var existente = await _operadorElectorRepository
            .GetByUserAndElectorAsync(operadorId, dto.ElectorId, includeInactive: true, cancellationToken);

        if (existente != null) //Para el caso de soft delete y reactivacion
        {
            existente.IsActive = true;
            existente.TenantId = tenantId;
            existente.DisponibleMiembroMesa = dto.DisponibleMiembroMesa;
            existente.RequiereTransporte = dto.RequiereTransporte;
            existente.NroTelefono = dto.NroTelefono;
            existente.DireccionRecogida = dto.DireccionRecogida;
            existente.Ubicacion = dto.Ubicacion is not null ? BuildUbicacion(dto.Ubicacion, tenantId) : null;
            existente.OperadorUbicacion = dto.OperadorUbicacion is not null ? BuildUbicacion(dto.OperadorUbicacion, tenantId) : null;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        var operadorElector = new OperadorElector
        {
            UserId                = operadorId,
            ElectorId             = dto.ElectorId,
            TenantId              = tenantId,
            DisponibleMiembroMesa = dto.DisponibleMiembroMesa,
            RequiereTransporte    = dto.RequiereTransporte,
            NroTelefono           = dto.NroTelefono,
            DireccionRecogida     = dto.DireccionRecogida,
            Ubicacion             = dto.Ubicacion is not null ? BuildUbicacion(dto.Ubicacion, tenantId) : null,
            OperadorUbicacion     = dto.OperadorUbicacion is not null ? BuildUbicacion(dto.OperadorUbicacion, tenantId) : null
        };

        await _operadorElectorRepository.AddAsync(operadorElector, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private Ubicacion BuildUbicacion(UbicacionInputDto dto, int tenantId) => new()
    {
        Lat         = dto.Lat,
        Lng         = dto.Lng,
        Descripcion = dto.Descripcion,
        TenantId    = tenantId,
        CreatedBy   = _currentUserService.UserId
    };
}
