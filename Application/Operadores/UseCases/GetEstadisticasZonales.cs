using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class GetEstadisticasZonales
{
    private readonly IEstadisticasZonalesRepository _repository;
    private readonly ITenantService                 _tenantService;
    private readonly ICurrentUserService            _currentUserService;

    public GetEstadisticasZonales(
        IEstadisticasZonalesRepository repository,
        ITenantService tenantService,
        ICurrentUserService currentUserService)
    {
        _repository         = repository;
        _tenantService      = tenantService;
        _currentUserService = currentUserService;
    }

    private (int tenantId, int? coordinatorId) ResolveScope()
    {
        var tenantId = _tenantService.GetCurrentTenantId();
        int? coordinatorId = _currentUserService.Role switch
        {
            Admin       => null,
            Coordinator => _currentUserService.UserId,
            _           => throw new UnauthorizedAccessException("No tiene permiso para ver esta información.")
        };
        return (tenantId, coordinatorId);
    }

    public async Task<IEnumerable<SeccionalCaptacionDto>> GetListaAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, coordinatorId) = ResolveScope();
        return await _repository.GetSecccionalesCaptacionAsync(tenantId, coordinatorId, cancellationToken);
    }

    public async Task<ResumenZonalCaptacionDto> GetResumenAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, coordinatorId) = ResolveScope();
        var lista = (await _repository.GetSecccionalesCaptacionAsync(tenantId, coordinatorId, cancellationToken)).ToList();

        if (lista.Count == 0)
            return new ResumenZonalCaptacionDto(null, null, 0);

        var mayor   = lista.MaxBy(s => s.TotalElectores);
        var menor   = lista.MinBy(s => s.TotalElectores);
        var promedio = lista.Average(s => s.TotalElectores);

        return new ResumenZonalCaptacionDto(mayor, menor, Math.Round(promedio, 2));
    }
}
