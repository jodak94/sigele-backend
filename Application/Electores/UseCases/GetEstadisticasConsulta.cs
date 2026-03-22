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

    public Task<EstadisticasConsultaDto> ExecuteAsync(CancellationToken cancellationToken = default)
        => _consultaRepository.GetEstadisticasAsync(cancellationToken);
}
