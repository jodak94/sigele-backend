using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Application.Padron;
using Application.Tenants.Interfaces;
using Application.Users.Interfaces;
using Application.VehiculoRequests.Interfaces;
using Domain.Entities;

namespace Application.Operadores.UseCases;

public class AsignarElector
{
    private readonly IOperadorPersonaRepository _operadorPersonaRepository;
    private readonly IUbicacionRepository _ubicacionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IPersonaRepository _personaRepository;
    private readonly IVehiculoRequestRepository _vehiculoRequestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AsignarElector(
        IOperadorPersonaRepository operadorPersonaRepository,
        IUbicacionRepository ubicacionRepository,
        IUserRepository userRepository,
        ITenantRepository tenantRepository,
        IPersonaRepository personaRepository,
        IVehiculoRequestRepository vehiculoRequestRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _operadorPersonaRepository = operadorPersonaRepository;
        _ubicacionRepository = ubicacionRepository;
        _userRepository = userRepository;
        _tenantRepository = tenantRepository;
        _personaRepository = personaRepository;
        _vehiculoRequestRepository = vehiculoRequestRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int operadorId, AsignarElectorDto dto, CancellationToken cancellationToken = default)
    {
        var tenantId = _currentUserService.TenantId;

        var tenant = await _tenantRepository.GetByIdAsync(tenantId, cancellationToken);
        if (tenant is null)
            throw new KeyNotFoundException("Tenant no encontrado.");

        if (tenant.CaptacionBloqueada)
            throw new InvalidOperationException("La captación está bloqueada porque se alcanzó el límite del plan.");

        var operador = await _userRepository.GetByIdAsync(operadorId, cancellationToken);
        if (operador is null || operador.TenantId != tenantId)
            throw new KeyNotFoundException("Operador no encontrado.");

        var yaAsignado = await _operadorPersonaRepository.PersonaActivaEnTenantAsync(dto.ElectorId, tenantId, cancellationToken);
        if (yaAsignado)
            throw new InvalidOperationException("El elector ya está asignado a otro operador en este tenant.");

        var existente = await _operadorPersonaRepository
            .GetByUserAndPersonaAsync(operadorId, dto.ElectorId, includeInactive: true, cancellationToken);

        if (existente != null)
        {
            existente.IsActive = true;
            existente.TenantId = tenantId;
            existente.DisponibleMiembroMesa = dto.DisponibleMiembroMesa;
            existente.RequiereTransporte = dto.RequiereTransporte;
            existente.NroTelefono = dto.NroTelefono;
            existente.DireccionRecogida = dto.DireccionRecogida;
            existente.Ubicacion = dto.Ubicacion is not null ? BuildUbicacion(dto.Ubicacion, tenantId) : null;
            existente.OperadorUbicacion = dto.OperadorUbicacion is not null ? BuildUbicacion(dto.OperadorUbicacion, tenantId) : null;

            if (dto.SolicitudAlquiler && dto.CapacidadVehiculo.HasValue)
                await AddVehiculoRequestAsync(dto, operadorId, tenantId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _tenantRepository.IncrementElectorCountAsync(tenantId, cancellationToken);
            return;
        }

        var operadorPersona = new OperadorPersona
        {
            UserId                = operadorId,
            Cedula                = dto.ElectorId,
            TenantId              = tenantId,
            DisponibleMiembroMesa = dto.DisponibleMiembroMesa,
            RequiereTransporte    = dto.RequiereTransporte,
            NroTelefono           = dto.NroTelefono,
            DireccionRecogida     = dto.DireccionRecogida,
            Ubicacion             = dto.Ubicacion is not null ? BuildUbicacion(dto.Ubicacion, tenantId) : null,
            OperadorUbicacion     = dto.OperadorUbicacion is not null ? BuildUbicacion(dto.OperadorUbicacion, tenantId) : null
        };

        await _operadorPersonaRepository.AddAsync(operadorPersona, cancellationToken);

        if (dto.SolicitudAlquiler && dto.CapacidadVehiculo.HasValue)
            await AddVehiculoRequestAsync(dto, operadorId, tenantId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _tenantRepository.IncrementElectorCountAsync(tenantId, cancellationToken);
    }

    private async Task AddVehiculoRequestAsync(AsignarElectorDto dto, int operadorId, int tenantId, CancellationToken cancellationToken)
    {
        var persona = await _personaRepository.GetByCedulaAsync(dto.ElectorId, cancellationToken);
        if (persona is null) return;

        var nombreDueno = $"{persona.Nombre} {persona.Apellido}".Trim();

        var solicitud = new VehiculoRequest
        {
            ElectorId     = dto.ElectorId,
            OperadorId    = operadorId,
            NombreDueno   = nombreDueno,
            TelefonoDueno = dto.NroTelefono,
            Capacidad     = dto.CapacidadVehiculo!.Value,
            MontoAlquiler = dto.MontoAlquilerVehiculo,
            Estado        = EstadoSolicitudVehiculo.Pendiente,
            TenantId      = tenantId,
            CreatedBy     = operadorId
        };

        await _vehiculoRequestRepository.AddAsync(solicitud, cancellationToken);
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
