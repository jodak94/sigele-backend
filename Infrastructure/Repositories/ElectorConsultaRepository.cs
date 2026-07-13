using Application.Electores.DTOs;
using Application.Electores.Interfaces;
using Dapper;
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

    public async Task<EstadisticasConsultaDto> GetEstadisticasAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        var ahora = DateTimeOffset.UtcNow;
        var inicioHoy  = new DateTimeOffset(ahora.UtcDateTime.Date, TimeSpan.Zero);
        var inicioAyer = inicioHoy.AddDays(-1);
        var inicio7D   = inicioHoy.AddDays(-7);

        var base_ = _context.ElectorConsultas.Where(c => c.TenantId == tenantId);

        var hoy    = await base_.CountAsync(c => c.ConsultadoEn >= inicioHoy,  cancellationToken);
        var ayer   = await base_.CountAsync(c => c.ConsultadoEn >= inicioAyer && c.ConsultadoEn < inicioHoy, cancellationToken);
        var siete  = await base_.CountAsync(c => c.ConsultadoEn >= inicio7D,   cancellationToken);
        var total  = await base_.CountAsync(cancellationToken);

        return new EstadisticasConsultaDto(hoy, ayer, siete, total);
    }

    public async Task<EstadisticasPadronPublicoDto> GetEstadisticasPadronPublicoAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        var base_ = _context.ElectorConsultas.Where(c => c.TenantId == tenantId);

        var inicio7D = new DateTimeOffset(DateTimeOffset.UtcNow.UtcDateTime.Date, TimeSpan.Zero).AddDays(-7);

        var total         = await base_.LongCountAsync(cancellationToken);
        var ultimosSiete  = await base_.LongCountAsync(c => c.ConsultadoEn >= inicio7D, cancellationToken);
        var cedulasUnicas = await base_.Select(c => c.Cedula).Distinct().LongCountAsync(cancellationToken);

        var horarioPico = await base_
            .GroupBy(c => c.ConsultadoEn.Hour)
            .Select(g => new { Hora = g.Key, Total = g.LongCount() })
            .OrderByDescending(x => x.Total)
            .Select(x => (int?)x.Hora)
            .FirstOrDefaultAsync(cancellationToken);

        return new EstadisticasPadronPublicoDto(total, ultimosSiete, cedulasUnicas, horarioPico);
    }

    public async Task<IEnumerable<TopLocalConsultadoDto>> GetTopLocalesConsultadosAsync(int tenantId, int top, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT loc.descrip AS LocalVotacion,
                   COUNT(*)::bigint AS TotalBusquedas
            FROM elector_consulta ec
            JOIN persona p ON p.cedula::text = ec.cedula
            JOIN LATERAL (
                SELECT i.depart, i.distrito, i.zona, i.local
                FROM inscripcion i
                WHERE i.cedula = p.cedula
                ORDER BY i.id DESC
                LIMIT 1
            ) ui ON true
            JOIN localidad loc ON loc.depart   = ui.depart
                               AND loc.distrito = ui.distrito
                               AND loc.zona     = ui.zona
                               AND loc.local    = ui.local
            WHERE ec.tenant_id   = @TenantId
              AND ec.encontrado  = true
              AND loc.descrip IS NOT NULL
            GROUP BY loc.descrip
            ORDER BY TotalBusquedas DESC
            LIMIT @Top
            """;

        var connection = _context.Database.GetDbConnection();
        return await connection.QueryAsync<TopLocalConsultadoDto>(sql, new { TenantId = tenantId, Top = top });
    }

    public async Task<IEnumerable<UltimaConsultaDto>> GetUltimasConsultasAsync(int tenantId, int top, CancellationToken cancellationToken = default)
    {
        return await _context.ElectorConsultas
            .Where(c => c.TenantId == tenantId)
            .OrderByDescending(c => c.ConsultadoEn)
            .Take(top)
            .Select(c => new UltimaConsultaDto(c.ConsultadoEn, c.Cedula, c.Encontrado))
            .ToListAsync(cancellationToken);
    }
}
