using Application.Common.Interfaces;
using Application.Electores.DTOs;
using Application.Electores.Interfaces;

namespace Application.Electores.UseCases;

public class GetTopLocalesConsultados
{
    private readonly IElectorConsultaRepository _consultaRepository;
    private readonly ITenantService             _tenantService;

    public GetTopLocalesConsultados(IElectorConsultaRepository consultaRepository, ITenantService tenantService)
    {
        _consultaRepository = consultaRepository;
        _tenantService      = tenantService;
    }

    public Task<IEnumerable<TopLocalConsultadoDto>> ExecuteAsync(int top, CancellationToken cancellationToken = default)
        => _consultaRepository.GetTopLocalesConsultadosAsync(_tenantService.GetCurrentTenantId(), top, cancellationToken);
}
