using Application.Asistencia.DTOs;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;

namespace Application.Asistencia.UseCases;

public class MarcarAsistencia
{
    private readonly IOperadorPersonaRepository _operadorPersonaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public MarcarAsistencia(IOperadorPersonaRepository operadorPersonaRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _operadorPersonaRepository = operadorPersonaRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int userId, int cedula, MarcarAsistenciaDto dto, CancellationToken cancellationToken = default)
    {
        var operadorPersona = await _operadorPersonaRepository.GetByUserAndPersonaAsync(userId, cedula, includeInactive: false, cancellationToken)
            ?? throw new KeyNotFoundException($"Elector {cedula} no encontrado para el operador {userId}.");

        if (operadorPersona.TenantId != _currentUserService.TenantId)
            throw new KeyNotFoundException($"Elector {cedula} no encontrado para el operador {userId}.");

        operadorPersona.Asistio = dto.Asistio;
        operadorPersona.AsistioMarcadoEn = dto.Asistio ? DateTimeOffset.UtcNow : null;
        operadorPersona.AsistioMarcadoPor = dto.Asistio ? _currentUserService.UserId : null;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
