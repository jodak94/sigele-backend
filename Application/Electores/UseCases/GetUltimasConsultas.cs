using Application.Common.Interfaces;
using Application.Electores.DTOs;
using Application.Electores.Interfaces;

namespace Application.Electores.UseCases;

public class GetUltimasConsultas
{
    private readonly IElectorConsultaRepository _consultaRepository;
    private readonly ITenantService             _tenantService;

    public GetUltimasConsultas(IElectorConsultaRepository consultaRepository, ITenantService tenantService)
    {
        _consultaRepository = consultaRepository;
        _tenantService      = tenantService;
    }

    public Task<IEnumerable<UltimaConsultaDto>> ExecuteAsync(int top, CancellationToken cancellationToken = default)
        => _consultaRepository.GetUltimasConsultasAsync(_tenantService.GetCurrentTenantId(), top, cancellationToken);
}
