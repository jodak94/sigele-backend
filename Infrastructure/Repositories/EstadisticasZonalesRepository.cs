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

    public async Task<IEnumerable<SeccionalCaptacionDto>> GetSecccionalesCaptacionAsync(int tenantId, int? coordinatorId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT e.codigo_sec  AS CodigoSeccional,
                   COUNT(*)::int AS TotalElectores
            FROM operador_elector oe
            JOIN elector e    ON e.id    = oe.elector_id
            JOIN "user"  u    ON u.id    = oe.user_id
            WHERE oe.tenant_id = @TenantId
              AND oe.is_active  = true
              AND (@CoordinatorId::int IS NULL OR u.coordinator_id = @CoordinatorId)
            GROUP BY e.codigo_sec
            ORDER BY TotalElectores DESC
            """;

        var connection = _context.Database.GetDbConnection();
        return await connection.QueryAsync<SeccionalCaptacionDto>(sql, new { TenantId = tenantId, CoordinatorId = coordinatorId });
    }
}
