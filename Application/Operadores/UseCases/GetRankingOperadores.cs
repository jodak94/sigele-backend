using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public record RankingOperadorDto(int UserId, string FullName, int TotalElectores);

public class GetRankingOperadores
{
    private readonly IOperadorPersonaRepository _repository;
    private readonly ICurrentUserService        _currentUserService;

    public GetRankingOperadores(IOperadorPersonaRepository repository, ICurrentUserService currentUserService)
    {
        _repository         = repository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<RankingOperadorDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var requesterRole = _currentUserService.Role;
        var tenantId      = _currentUserService.TenantId;

        int? coordinatorId = requesterRole switch
        {
            Admin       => null,
            Coordinator => _currentUserService.UserId,
            _           => throw new UnauthorizedAccessException("No tiene permiso para ver esta información.")
        };

        var operadores = await _repository.GetInfoDeOperadoresAsync(coordinatorId, tenantId, cancellationToken);

        return operadores
            .OrderByDescending(o => o.TotalElectores)
            .Select(o => new RankingOperadorDto(o.UserId, o.FullName, o.TotalElectores));
    }
}
