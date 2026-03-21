using Application.Electores.DTOs;
using Application.Electores.Interfaces;
using Domain.Entities;

namespace Application.Electores.UseCases;

public class GetElectorByNumeroCed
{
    private readonly IElectorRepository _electorRepository;
    private readonly IElectorConsultaRepository _consultaRepository;

    public GetElectorByNumeroCed(IElectorRepository electorRepository, IElectorConsultaRepository consultaRepository)
    {
        _electorRepository = electorRepository;
        _consultaRepository = consultaRepository;
    }

    public async Task<IEnumerable<ElectorDetailDto>> ExecuteAsync(int numeroCed, ConsultaContextDto? contexto, bool includeId = false, CancellationToken cancellationToken = default)
    {
        var results = (await _electorRepository.GetByNumeroCedAsync(numeroCed, cancellationToken)).ToList();

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

        return results.Select(r => new ElectorDetailDto
        {
            Id        = includeId ? r.Elector.Id : null,
            NumeroCed = r.Elector.NumeroCed,
            Apellido  = r.Elector.Apellido,
            Nombre    = r.Elector.Nombre,
            Direccion = r.Elector.Direccion,
            FechaNaci = r.Elector.FechaNaci,
            Mesa      = r.Elector.Mesa,
            Orden     = r.Elector.Orden,
            CodigoSex = r.Elector.CodigoSex,
            Local = r.Elector.Local is null ? null : new LocalDto
            {
                SeccLoc   = r.Elector.Local.SeccLoc,
                NombreLoc = r.Elector.Local.NombreLoc,
                Direccion = r.Elector.Local.Direccion
            },
            Seccional = r.Seccional is null ? null : new SeccionalDto
            {
                NDepart    = r.Seccional.NDepart,
                NDistrito  = r.Seccional.NDistrito,
                Descripcio = r.Seccional.Descripcio,
                Direccion  = r.Seccional.Direccion
            }
        });
    }
}
