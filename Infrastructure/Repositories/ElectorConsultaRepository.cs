using Application.Electores.DTOs;
using Application.Electores.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ElectorConsultaRepository : IElectorConsultaRepository
{
    private readonly AppDbContext _context;

    public ElectorConsultaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(ElectorConsulta consulta, CancellationToken cancellationToken = default)
    {
        _context.ElectorConsultas.Add(consulta);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<EstadisticasConsultaDto> GetEstadisticasAsync(CancellationToken cancellationToken = default)
    {
        var ahora = DateTimeOffset.UtcNow;
        var inicioHoy  = new DateTimeOffset(ahora.UtcDateTime.Date, TimeSpan.Zero);
        var inicioAyer = inicioHoy.AddDays(-1);
        var inicio7D   = inicioHoy.AddDays(-7);

        var base_ = _context.ElectorConsultas;

        var hoy    = await base_.CountAsync(c => c.ConsultadoEn >= inicioHoy,  cancellationToken);
        var ayer   = await base_.CountAsync(c => c.ConsultadoEn >= inicioAyer && c.ConsultadoEn < inicioHoy, cancellationToken);
        var siete  = await base_.CountAsync(c => c.ConsultadoEn >= inicio7D,   cancellationToken);
        var total  = await base_.CountAsync(cancellationToken);

        return new EstadisticasConsultaDto(hoy, ayer, siete, total);
    }
}
