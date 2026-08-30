using Application.Common.Interfaces;
using Application.Electores.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PersonaPadronRepository : IElectorPadronRepository
{
    private readonly AppDbContext _context;

    public PersonaPadronRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ElectorDetailDto>> GetByNumeroCedAsync(int numeroCed, bool includeId, CancellationToken cancellationToken = default)
    {
        var result = await _context.Personas
            .Where(p => p.Cedula == numeroCed)
            .Select(p => new ElectorDetailDto
            {
                Id        = includeId ? (int?)p.Cedula : null,
                NumeroCed = p.Cedula,
                Nombre    = p.Nombre,
                Apellido  = p.Apellido,
                FechaNaci = p.FecNac,
                CodigoSex = p.Sexo == "M" ? (short?)1 : p.Sexo == "F" ? (short?)2 : null,
                Direccion = null,
                Mesa      = p.MesaOrden != null ? p.MesaOrden.Mesa : (short?)null,
                Orden     = p.MesaOrden != null ? p.MesaOrden.Orden : (short?)null,
                Local = p.Inscripciones
                    .OrderByDescending(i => i.Id)
                    .Select(i => new LocalDto
                    {
                        CodigoLocal = (int)(i.Local ?? 0),
                        NombreLoc   = i.Localidad != null ? i.Localidad.Descrip : null,
                        Direccion   = null
                    })
                    .FirstOrDefault(),
                Zona = p.Inscripciones
                    .OrderByDescending(i => i.Id)
                    .Select(i => new ZonaDto
                    {
                        Depart         = i.Depart,
                        Distrito       = i.Distrito,
                        Zona           = i.Zona,
                        NombreDepart   = i.Localidad != null && i.Localidad.Dpto != null ? i.Localidad.Dpto.Descrip : null,
                        NombreDistrito = i.Localidad != null && i.Localidad.DistNav != null ? i.Localidad.DistNav.Descrip : null,
                        NombreZona     = i.ZonaNav != null ? i.ZonaNav.Descrip : null
                    })
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}
