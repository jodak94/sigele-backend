using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class GetElectorUbicaciones
{
    private readonly IOperadorPersonaRepository _operadorElectorRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetElectorUbicaciones(
        IOperadorPersonaRepository operadorElectorRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ElectorUbicacionDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var userId   = _currentUserService.UserId;
        var tenantId = _currentUserService.TenantId;
        var role     = _currentUserService.Role;

        var (operadorId, coordinadorId) = role switch
        {
            Operator    => (userId, (int?)null),
            Coordinator => ((int?)null, userId),
            _           => ((int?)null, (int?)null)
        };

        return await _operadorElectorRepository.GetElectorUbicacionesAsync(
            operadorId, coordinadorId, tenantId, cancellationToken);
    }
}
