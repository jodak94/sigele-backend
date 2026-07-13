using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class GetInfoDeOperadores
{
    private readonly IOperadorPersonaRepository _operadorElectorRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetInfoDeOperadores(
        IOperadorPersonaRepository operadorElectorRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<OperadorInfoDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var requesterId = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;
        var tenantId = _currentUserService.TenantId;

        int? coordinatorId = requesterRole switch
        {
            Admin       => null,
            Coordinator => requesterId,
            _           => throw new UnauthorizedAccessException("No tiene permiso para ver esta información.")
        };

        return await _operadorElectorRepository.GetInfoDeOperadoresAsync(coordinatorId, tenantId, cancellationToken);
    }
}
