using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class GetResumenCoordinadores
{
    private readonly IOperadorPersonaRepository _repository;
    private readonly ICurrentUserService        _currentUserService;

    public GetResumenCoordinadores(IOperadorPersonaRepository repository, ICurrentUserService currentUserService)
    {
        _repository         = repository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ResumenCoordinadorDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserService.Role != Admin)
            throw new UnauthorizedAccessException("Solo el administrador puede ver el resumen de coordinadores.");

        return await _repository.GetResumenCoordinadoresAsync(_currentUserService.TenantId, cancellationToken);
    }
}
