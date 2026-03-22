using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class BuscarElectorAsignado
{
    private readonly IOperadorElectorRepository _operadorElectorRepository;
    private readonly ICurrentUserService _currentUserService;

    public BuscarElectorAsignado(
        IOperadorElectorRepository operadorElectorRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ElectorAsignadoDto>> ExecuteAsync(int numeroCed, CancellationToken cancellationToken = default)
    {
        var requesterId = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;
        var tenantId = _currentUserService.TenantId;

        var (operatorId, coordinatorId) = requesterRole switch
        {
            Operator    => ((int?)requesterId, (int?)null),
            Coordinator => (null, (int?)requesterId),
            Admin       => (null, (int?)null),
            _           => throw new UnauthorizedAccessException("No tiene permiso para realizar esta búsqueda.")
        };

        return await _operadorElectorRepository.BuscarPorNumeroCedAsync(numeroCed, operatorId, coordinatorId, tenantId, cancellationToken);
    }
}
