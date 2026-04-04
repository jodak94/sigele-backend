using Application.Auditoria.Constants;
using Application.Auditoria.DTOs;
using Application.Auditoria.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AuditoriaRepository : IAuditoriaRepository
{
    private readonly AppDbContext _context;

    public AuditoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    // 30+ captaciones en una ventana de 15 minutos por el mismo operador
    public async Task<IEnumerable<AlertaOperadorDto>> GetCaptacionesRapidasAsync(
        int tenantId, int? coordinadorId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId)
            .Where(oe => oe.Tenant.OnboardingUntil == null || oe.CreatedAt > oe.Tenant.OnboardingUntil)
            .Where(oe => coordinadorId == null || oe.User!.CoordinatorId == coordinadorId)
            .Where(oe => _context.OperadorElectores.Count(oe2 =>
                oe2.UserId == oe.UserId &&
                oe2.TenantId == oe.TenantId &&
                oe2.CreatedAt >= oe.CreatedAt - TimeSpan.FromMinutes(15) &&
                oe2.CreatedAt <= oe.CreatedAt) >= 30)
            .Select(oe => new AlertaOperadorDto(oe.UserId, oe.User!.FullName, oe.User!.Email))
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    // 15+ captaciones desde la misma coordenada por el mismo operador
    public async Task<IEnumerable<AlertaOperadorDto>> GetMismaCoordenadaAsync(
        int tenantId, int? coordinadorId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId)
            .Where(oe => oe.OperadorUbicacion != null)
            .Where(oe => oe.Tenant.OnboardingUntil == null || oe.CreatedAt > oe.Tenant.OnboardingUntil)
            .Where(oe => coordinadorId == null || oe.User!.CoordinatorId == coordinadorId)
            .GroupBy(oe => new { oe.UserId, oe.User!.FullName, oe.User!.Email, oe.OperadorUbicacion!.Lat, oe.OperadorUbicacion!.Lng })
            .Where(g => g.Count() >= 15)
            .Select(g => new AlertaOperadorDto(g.Key.UserId, g.Key.FullName, g.Key.Email))
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    // Captaciones entre 00:00 y 05:00 hora Paraguay (UTC-4, fijo desde que PY eliminó el horario de verano)
    public async Task<IEnumerable<AlertaOperadorDto>> GetFueraHorarioAsync(
        int tenantId, int? coordinadorId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId)
            .Where(oe => oe.Tenant.OnboardingUntil == null || oe.CreatedAt > oe.Tenant.OnboardingUntil)
            .Where(oe => coordinadorId == null || oe.User!.CoordinatorId == coordinadorId)
            .Where(oe => oe.CreatedAt.Hour >= 3 && oe.CreatedAt.Hour < 8) // 00:00-04:59 PY = 04:00-08:59 UTC
            .Select(oe => new AlertaOperadorDto(oe.UserId, oe.User!.FullName, oe.User!.Email))
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    // Captaciones sin ubicación del operador
    public async Task<IEnumerable<AlertaOperadorDto>> GetUbicacionDenegadaAsync(
        int tenantId, int? coordinadorId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId)
            .Where(oe => oe.Tenant.OnboardingUntil == null || oe.CreatedAt > oe.Tenant.OnboardingUntil)
            .Where(oe => coordinadorId == null || oe.User!.CoordinatorId == coordinadorId)
            .Where(oe => oe.OperadorUbicacionId == null)
            .Select(oe => new AlertaOperadorDto(oe.UserId, oe.User!.FullName, oe.User!.Email))
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    // Captaciones de un operador filtradas por tipo de alerta
    public async Task<IEnumerable<CaptacionDetalleDto>> GetCaptacionesDeOperadorAsync(
        int tenantId, int operadorId, string tipoAlerta, CancellationToken cancellationToken = default)
    {
        // Para fuera_horario y ubicacion_denegada el filtro es simple, se resuelve en SQL
        if (tipoAlerta == TiposAlerta.FueraHorario || tipoAlerta == TiposAlerta.UbicacionDenegada)
        {
            var sqlQuery = _context.OperadorElectores
                .Where(oe => oe.TenantId == tenantId && oe.UserId == operadorId);

            sqlQuery = tipoAlerta == TiposAlerta.FueraHorario
                ? sqlQuery.Where(oe => oe.CreatedAt.Hour >= 4 && oe.CreatedAt.Hour < 9)
                : sqlQuery.Where(oe => oe.OperadorUbicacionId == null);

            return await sqlQuery
                .OrderByDescending(oe => oe.CreatedAt)
                .Select(oe => new CaptacionDetalleDto(
                    oe.Elector.Nombre,
                    oe.Elector.Apellido,
                    oe.Elector.NumeroCed,
                    oe.CreatedAt
                ))
                .ToListAsync(cancellationToken);
        }

        // Para captaciones_rapidas y misma_coordenada necesitamos evaluar ventanas,
        // cargamos en memoria y filtramos con sliding window
        var todas = await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId && oe.UserId == operadorId)
            .OrderBy(oe => oe.CreatedAt)
            .Select(oe => new
            {
                ElectorNombre   = oe.Elector.Nombre,
                ElectorApellido = oe.Elector.Apellido,
                ElectorCedula   = oe.Elector.NumeroCed,
                oe.CreatedAt,
                UbicacionLat = oe.OperadorUbicacion != null ? (double?)oe.OperadorUbicacion.Lat : null,
                UbicacionLng = oe.OperadorUbicacion != null ? (double?)oe.OperadorUbicacion.Lng : null,
            })
            .ToListAsync(cancellationToken);

        IEnumerable<int> indicesAlertados = tipoAlerta switch
        {
            TiposAlerta.CaptacionesRapidas => GetIndicesCaptacionesRapidas(todas.Select(x => x.CreatedAt).ToList()),
            TiposAlerta.MismaCoordenada    => GetIndicesMismaCoordenada(todas.Select(x => (x.UbicacionLat, x.UbicacionLng)).ToList()),
            _ => throw new ArgumentException($"Tipo de alerta desconocido: {tipoAlerta}")
        };

        var indicesSet = indicesAlertados.ToHashSet();

        return todas
            .Where((_, i) => indicesSet.Contains(i))
            .Select(x => new CaptacionDetalleDto(x.ElectorNombre, x.ElectorApellido, x.ElectorCedula, x.CreatedAt))
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }

    // Ubicaciones del operador durante sus captaciones para el mapa
    public async Task<IEnumerable<MapaOperadorDto>> GetMapaOperadorAsync(
        int tenantId, int operadorId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId && oe.UserId == operadorId)
            .Where(oe => oe.OperadorUbicacion != null)
            .OrderByDescending(oe => oe.CreatedAt)
            .Select(oe => new MapaOperadorDto(
                oe.UserId,
                oe.User!.FullName,
                oe.User!.Email,
                oe.ElectorId,
                oe.Elector.Nombre,
                oe.Elector.Apellido,
                oe.Elector.NumeroCed,
                oe.OperadorUbicacion!.Lat,
                oe.OperadorUbicacion!.Lng,
                oe.OperadorUbicacion!.Descripcion,
                oe.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }

    // Retorna los índices de registros que forman parte de una ráfaga (30+ en 15 min)
    private static IEnumerable<int> GetIndicesCaptacionesRapidas(List<DateTime> tiempos)
    {
        var enBurst = new HashSet<int>();
        int left = 0;

        for (int right = 0; right < tiempos.Count; right++)
        {
            while ((tiempos[right] - tiempos[left]).TotalMinutes > 15)
                left++;

            if (right - left + 1 >= 30)
                for (int i = left; i <= right; i++)
                    enBurst.Add(i);
        }

        return enBurst;
    }

    // Retorna los índices de registros cuya coordenada aparece 15+ veces
    private static IEnumerable<int> GetIndicesMismaCoordenada(List<(double? Lat, double? Lng)> coords)
    {
        var conteo = coords
            .Where(c => c.Lat.HasValue && c.Lng.HasValue)
            .GroupBy(c => (c.Lat!.Value, c.Lng!.Value))
            .Where(g => g.Count() >= 15)
            .Select(g => g.Key)
            .ToHashSet();

        return coords
            .Select((c, i) => (c, i))
            .Where(x => x.c.Lat.HasValue && conteo.Contains((x.c.Lat!.Value, x.c.Lng!.Value)))
            .Select(x => x.i);
    }
}
