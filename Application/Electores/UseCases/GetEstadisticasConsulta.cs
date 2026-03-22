using Application.Common.Interfaces;
using Application.Electores.DTOs;
using Application.Electores.Interfaces;

namespace Application.Electores.UseCases;

public class GetEstadisticasConsulta
{
    private readonly IElectorConsultaRepository _consultaRepository;
    private readonly ITenantService _tenantService;

    public GetEstadisticasConsulta(IElectorConsultaRepository consultaRepository, ITenantService tenantService)
    {
        _consultaRepository = consultaRepository;
        _tenantService = tenantService;
    }

    public Task<EstadisticasConsultaDto> ExecuteAsync(CancellationToken cancellationToken = default)
        => _consultaRepository.GetEstadisticasAsync(_tenantService.GetCurrentTenantId(), cancellationToken);
}
