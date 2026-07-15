using Application.Asistencia.DTOs;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;

namespace Application.Asistencia.UseCases;

public class GetAsistenciaResumen
{
    private readonly IOperadorPersonaRepository _operadorPersonaRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAsistenciaResumen(IOperadorPersonaRepository operadorPersonaRepository, ICurrentUserService currentUserService)
    {
        _operadorPersonaRepository = operadorPersonaRepository;
        _currentUserService = currentUserService;
    }

    public Task<AsistenciaResumenDto> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return _operadorPersonaRepository.GetAsistenciaResumenAsync(_currentUserService.TenantId, cancellationToken);
    }
}
