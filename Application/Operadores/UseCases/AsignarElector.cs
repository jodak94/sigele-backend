using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Application.Users.Interfaces;
using Domain.Entities;

namespace Application.Operadores.UseCases;

public class AsignarElector
{
    private readonly IOperadorElectorRepository _operadorElectorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AsignarElector(
        IOperadorElectorRepository operadorElectorRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
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

        var operadorElector = new OperadorElector
        {
            UserId                = operadorId,
            ElectorId             = dto.ElectorId,
            TenantId              = tenantId,
            DisponibleMiembroMesa = dto.DisponibleMiembroMesa,
            RequiereTransporte    = dto.RequiereTransporte,
            NroTelefono           = dto.NroTelefono,
            DireccionRecogida     = dto.DireccionRecogida
        };

        await _operadorElectorRepository.AddAsync(operadorElector, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
