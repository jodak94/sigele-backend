using Application.Common.Interfaces;
using Application.Electores.DTOs;
using Application.Electores.Interfaces;

namespace Application.Electores.UseCases;

public class GetEstadisticasPadronPublico
{
    private readonly IElectorConsultaRepository _consultaRepository;
    private readonly ITenantService             _tenantService;

    public GetEstadisticasPadronPublico(IElectorConsultaRepository consultaRepository, ITenantService tenantService)
    {
        _consultaRepository = consultaRepository;
        _tenantService      = tenantService;
    }

    public Task<EstadisticasPadronPublicoDto> ExecuteAsync(CancellationToken cancellationToken = default)
        => _consultaRepository.GetEstadisticasPadronPublicoAsync(_tenantService.GetCurrentTenantId(), cancellationToken);
}
