using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Dapper;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EstadisticasZonalesRepository : IEstadisticasZonalesRepository
{
    private readonly AppDbContext _context;

    public EstadisticasZonalesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ZonaCaptacionDto>> GetZonasCaptacionAsync(int tenantId, int? coordinatorId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT z.depart      AS Depart,
                   z.distrito    AS Distrito,
                   z.zona        AS Zona,
                   z.descrip     AS Descripcion,
                   COUNT(*)::int AS TotalElectores
            FROM operador_persona oe
            JOIN "user" u ON u.id = oe.user_id
            JOIN LATERAL (
                SELECT i.depart, i.distrito, i.zona
                FROM inscripcion i
                WHERE i.cedula = oe.cedula
                ORDER BY i.id DESC
                LIMIT 1
            ) ui ON true
            JOIN zona z ON z.depart = ui.depart AND z.distrito = ui.distrito AND z.zona = ui.zona
            WHERE oe.tenant_id = @TenantId
              AND oe.is_active  = true
              AND (@CoordinatorId::int IS NULL OR u.coordinator_id = @CoordinatorId)
            GROUP BY z.depart, z.distrito, z.zona, z.descrip
            ORDER BY TotalElectores DESC
            """;

        var connection = _context.Database.GetDbConnection();
        return await connection.QueryAsync<ZonaCaptacionDto>(sql, new { TenantId = tenantId, CoordinatorId = coordinatorId });
    }
}
