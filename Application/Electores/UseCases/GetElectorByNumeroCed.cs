using Application.Electores.DTOs;
using Application.Electores.Interfaces;

namespace Application.Electores.UseCases;

public class GetElectorByNumeroCed
{
    private readonly IElectorRepository _electorRepository;

    public GetElectorByNumeroCed(IElectorRepository electorRepository)
    {
        _electorRepository = electorRepository;
    }

    public async Task<IEnumerable<ElectorDetailDto>> ExecuteAsync(int numeroCed, CancellationToken cancellationToken = default)
    {
        var results = await _electorRepository.GetByNumeroCedAsync(numeroCed, cancellationToken);

        return results.Select(r => new ElectorDetailDto
        {
            NumeroCed = r.Elector.NumeroCed,
            Apellido = r.Elector.Apellido,
            Nombre = r.Elector.Nombre,
            Direccion = r.Elector.Direccion,
            FechaNaci = r.Elector.FechaNaci,
            Mesa = r.Elector.Mesa,
            Orden = r.Elector.Orden,
            CodigoSex = r.Elector.CodigoSex,
            Local = r.Elector.Local is null ? null : new LocalDto
            {
                SeccLoc = r.Elector.Local.SeccLoc,
                NombreLoc = r.Elector.Local.NombreLoc,
                Direccion = r.Elector.Local.Direccion
            },
            Seccional = r.Seccional is null ? null : new SeccionalDto
            {
                NDepart = r.Seccional.NDepart,
                NDistrito = r.Seccional.NDistrito,
                Descripcio = r.Seccional.Descripcio,
                Direccion = r.Seccional.Direccion
            }
        });
    }
}
