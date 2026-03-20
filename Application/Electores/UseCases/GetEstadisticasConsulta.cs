using Application.Electores.DTOs;
using Application.Electores.Interfaces;

namespace Application.Electores.UseCases;

public class GetEstadisticasConsulta
{
    private readonly IElectorConsultaRepository _consultaRepository;

    public GetEstadisticasConsulta(IElectorConsultaRepository consultaRepository)
    {
        _consultaRepository = consultaRepository;
    }

    public Task<EstadisticasConsultaDto> ExecuteAsync(int tenantId, CancellationToken cancellationToken = default)
        => _consultaRepository.GetEstadisticasAsync(tenantId, cancellationToken);
}
