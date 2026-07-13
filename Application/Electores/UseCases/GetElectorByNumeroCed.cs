using Application.Common.Interfaces;
using Application.Electores.DTOs;
using Application.Electores.Interfaces;
using Domain.Entities;

namespace Application.Electores.UseCases;

public class GetElectorByNumeroCed
{
    private readonly IElectorPadronRepository _padronRepository;
    private readonly IElectorConsultaRepository _consultaRepository;

    public GetElectorByNumeroCed(IElectorPadronRepository padronRepository, IElectorConsultaRepository consultaRepository)
    {
        _padronRepository = padronRepository;
        _consultaRepository = consultaRepository;
    }

    public async Task<IEnumerable<ElectorDetailDto>> ExecuteAsync(int numeroCed, ConsultaContextDto? contexto, bool includeId = false, CancellationToken cancellationToken = default)
    {
        var results = (await _padronRepository.GetByNumeroCedAsync(numeroCed, includeId, cancellationToken)).ToList();

        if (contexto is not null)
        {
            await _consultaRepository.RegistrarAsync(new ElectorConsulta
            {
                TenantId   = contexto.TenantId,
                Cedula     = numeroCed.ToString(),
                IpCliente  = contexto.IpCliente,
                UserAgent  = contexto.UserAgent,
                Origin     = contexto.Origin,
                Host       = contexto.Host,
                MetodoHttp = contexto.MetodoHttp,
                Encontrado = results.Count != 0
            }, cancellationToken);
        }

        return results;
    }
}
