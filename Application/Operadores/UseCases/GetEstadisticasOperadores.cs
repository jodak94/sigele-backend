using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class GetEstadisticasOperadores
{
    private readonly IEstadisticasOperadoresRepository _estadisticasRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetEstadisticasOperadores(
        IEstadisticasOperadoresRepository estadisticasRepository,
        ICurrentUserService currentUserService)
    {
        _estadisticasRepository = estadisticasRepository;
        _currentUserService = currentUserService;
    }

    public async Task<EstadisticasOperadoresDto> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var requesterId   = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;
        var tenantId      = _currentUserService.TenantId;

        int? coordinatorId = requesterRole switch
        {
            Admin       => null,
            Coordinator => requesterId,
            _           => throw new UnauthorizedAccessException("No tiene permiso para ver estas estadísticas.")
        };

        return await _estadisticasRepository.GetEstadisticasAsync(coordinatorId, tenantId, cancellationToken);
    }
}
