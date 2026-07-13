using Application.Common.Constants;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Dapper;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EstadisticasOperadoresRepository : IEstadisticasOperadoresRepository
{
    private readonly AppDbContext _context;

    public EstadisticasOperadoresRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EstadisticasOperadoresDto> GetEstadisticasAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                COUNT(*) FILTER (WHERE oe.is_active = true)::int                                       AS Activos,
                COUNT(*) FILTER (WHERE oe.is_active = true AND oe.disponible_miembro_mesa = true)::int AS CandidatosMesa,
                COUNT(*) FILTER (WHERE oe.is_active = true AND oe.requiere_transporte = true)::int     AS RequierenTransporte,
                COUNT(*) FILTER (WHERE oe.is_active = false)::int                                      AS Borrados
            FROM operador_persona oe
            JOIN "user" u ON u.id = oe.user_id
            JOIN role r   ON r.id = u.role_id
            WHERE oe.tenant_id   = @TenantId
              AND r.name         = @OperatorRole
              AND u.is_active    = true
              AND (@CoordinatorId::int IS NULL OR u.coordinator_id = @CoordinatorId)
            """;

        var connection = _context.Database.GetDbConnection();

        return await connection.QuerySingleAsync<EstadisticasOperadoresDto>(sql, new
        {
            TenantId      = tenantId,
            OperatorRole  = Roles.Operator,
            CoordinatorId = coordinatorId
        });
    }
}
