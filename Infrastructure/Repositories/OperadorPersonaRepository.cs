using Application.Asistencia.DTOs;
using Application.Common.Constants;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OperadorPersonaRepository : IOperadorPersonaRepository
{
    private readonly AppDbContext _context;

    public OperadorPersonaRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> PersonaActivaEnTenantAsync(int cedula, int tenantId, CancellationToken cancellationToken = default)
    {
        return _context.OperadorPersonas
            .AnyAsync(op => op.Cedula == cedula && op.TenantId == tenantId && op.IsActive, cancellationToken);
    }

    public async Task AddAsync(OperadorPersona op, CancellationToken cancellationToken = default)
    {
        await _context.OperadorPersonas.AddAsync(op, cancellationToken);
    }

    public Task<OperadorPersona?> GetAsync(int operadorId, int cedula, CancellationToken cancellationToken = default)
    {
        return _context.OperadorPersonas
            .Include(op => op.Ubicacion)
            .FirstOrDefaultAsync(op => op.UserId == operadorId && op.Cedula == cedula, cancellationToken);
    }

    public async Task<IEnumerable<OperadorElectorDto>> GetByOperadorAsync(int operadorId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorPersonas
            .Where(op => op.UserId == operadorId && op.IsActive)
            .Select(op => new OperadorElectorDto(
                op.Cedula,
                op.Persona.Nombre,
                op.Persona.Apellido,
                op.Cedula,
                op.DisponibleMiembroMesa,
                op.RequiereTransporte,
                op.NroTelefono,
                op.DireccionRecogida,
                op.Persona.Inscripciones.OrderByDescending(i => i.Id)
                    .Select(i => i.Localidad != null ? i.Localidad.Descrip : null)
                    .FirstOrDefault(),
                null,
                null,
                op.Ubicacion != null
                    ? new UbicacionDto(op.Ubicacion.Lat, op.Ubicacion.Lng, op.Ubicacion.Descripcion)
                    : null,
                op.Persona.Inscripciones.OrderByDescending(i => i.Id)
                    .Select(i => i.Localidad != null && i.Localidad.DistNav != null ? i.Localidad.DistNav.Descrip : null)
                    .FirstOrDefault(),
                op.Persona.Inscripciones.OrderByDescending(i => i.Id)
                    .Select(i => i.Localidad != null && i.Localidad.Dpto != null ? i.Localidad.Dpto.Descrip : null)
                    .FirstOrDefault(),
                null,
                null
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ElectorAsignadoDto>> BuscarPorNumeroCedAsync(int numeroCed, int? operatorId, int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        var query = _context.OperadorPersonas
            .Where(op => op.IsActive && op.TenantId == tenantId && op.Cedula == numeroCed);

        if (operatorId.HasValue)
            query = query.Where(op => op.UserId == operatorId.Value);
        else if (coordinatorId.HasValue)
            query = query.Where(op => op.User!.CoordinatorId == coordinatorId.Value);

        return await query
            .Select(op => new ElectorAsignadoDto(
                op.Cedula,
                op.Persona.Nombre,
                op.Persona.Apellido,
                op.Cedula,
                op.DisponibleMiembroMesa,
                op.RequiereTransporte,
                op.NroTelefono,
                op.DireccionRecogida,
                new OperadorBasicoDto(op.UserId, op.User!.FullName, op.User.Email, op.User.Phone)
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ResumenOperadorItemDto>> GetResumenOperadoresAsync(int? coordinatorId, int tenantId, short? codigoSeccional, CancellationToken cancellationToken = default)
    {
        var query = _context.Users
            .Where(u => u.TenantId == tenantId && u.IsActive && u.Role.Name == Roles.Operator);

        if (coordinatorId.HasValue)
            query = query.Where(u => u.CoordinatorId == coordinatorId.Value);

        return await query
            .Select(u => new ResumenOperadorItemDto(
                u.FullName,
                u.Phone,
                _context.OperadorPersonas.Count(op => op.UserId == u.Id && op.IsActive),
                _context.OperadorPersonas.Count(op => op.UserId == u.Id && op.IsActive && op.DisponibleMiembroMesa),
                _context.OperadorPersonas.Count(op => op.UserId == u.Id && op.IsActive && op.RequiereTransporte)
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CandidatoMesaFlatItemDto>> GetCandidatosMesaFlatAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorPersonas
            .Where(op => op.TenantId == tenantId &&
                         op.DisponibleMiembroMesa &&
                         (!coordinatorId.HasValue || op.User!.CoordinatorId == coordinatorId.Value))
            .Select(op => new CandidatoMesaFlatItemDto(
                op.Persona.Inscripciones.OrderByDescending(i => i.Id)
                    .Select(i => i.Localidad != null ? i.Localidad.Descrip : null)
                    .FirstOrDefault(),
                (op.Persona.Apellido + " " + op.Persona.Nombre).Trim(),
                op.Cedula,
                op.NroTelefono,
                null,
                op.User!.FullName
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DiaDFlatItemDto>> GetDiaDFlatAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorPersonas
            .Where(op => op.TenantId == tenantId &&
                         (!coordinatorId.HasValue || op.User!.CoordinatorId == coordinatorId.Value))
            .Select(op => new DiaDFlatItemDto(
                op.Persona.Inscripciones.OrderByDescending(i => i.Id)
                    .Select(i => i.Localidad != null ? i.Localidad.Descrip : null)
                    .FirstOrDefault(),
                null,
                null,
                op.Cedula,
                (op.Persona.Apellido + " " + op.Persona.Nombre).Trim(),
                op.NroTelefono,
                op.DireccionRecogida,
                op.RequiereTransporte,
                op.User!.FullName
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OperadorInfoDto>> GetInfoDeOperadoresAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        var query = _context.Users
            .Where(u => u.TenantId == tenantId && u.IsActive && u.Role.Name == Roles.Operator);

        if (coordinatorId.HasValue)
            query = query.Where(u => u.CoordinatorId == coordinatorId.Value);

        return await query
            .Select(u => new OperadorInfoDto(
                u.Id,
                u.FullName,
                u.Email,
                u.Phone,
                _context.OperadorPersonas.Count(op => op.UserId == u.Id && op.IsActive),
                _context.OperadorPersonas.Count(op => op.UserId == u.Id && op.IsActive && op.DisponibleMiembroMesa),
                _context.OperadorPersonas.Count(op => op.UserId == u.Id && op.IsActive && op.RequiereTransporte)
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ResumenCoordinadorDto>> GetResumenCoordinadoresAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.TenantId == tenantId && u.IsActive && u.Role.Name == Roles.Coordinator)
            .Select(u => new ResumenCoordinadorDto(
                u.Id,
                u.FullName,
                u.Email,
                u.Phone,
                _context.Users.Count(op => op.CoordinatorId == u.Id && op.Role.Name == Roles.Operator),
                _context.OperadorPersonas.Count(op => op.User!.CoordinatorId == u.Id && op.IsActive),
                _context.OperadorPersonas.Count(op => op.User!.CoordinatorId == u.Id && op.IsActive && op.DisponibleMiembroMesa)
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<OperadorPersona?> GetByUserAndPersonaAsync(int userId, int cedula, bool includeInactive, CancellationToken cancellationToken = default)
    {
        IQueryable<OperadorPersona> query = _context.OperadorPersonas;

        if (includeInactive)
            query = query.IgnoreQueryFilters();

        return await query
            .FirstOrDefaultAsync(op => op.UserId == userId && op.Cedula == cedula, cancellationToken);
    }

    public async Task<IEnumerable<ElectorUbicacionDto>> GetElectorUbicacionesAsync(int? operadorId, int? coordinadorId, int tenantId, CancellationToken cancellationToken = default)
    {
        var query = _context.OperadorPersonas
            .Where(op => op.TenantId == tenantId && op.UbicacionId != null);

        if (operadorId.HasValue)
            query = query.Where(op => op.UserId == operadorId.Value);
        else if (coordinadorId.HasValue)
            query = query.Where(op => op.User!.CoordinatorId == coordinadorId.Value);

        return await query
            .Select(op => new ElectorUbicacionDto(
                op.Cedula,
                op.Persona.Nombre,
                op.Persona.Apellido,
                op.Cedula,
                op.Ubicacion!.Lat,
                op.Ubicacion!.Lng,
                op.Ubicacion!.Descripcion
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<AsistenciaElectorDto> Items, int TotalCount)> GetAsistenciaListAsync(int tenantId, string? search, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.OperadorPersonas
            .Where(op => op.TenantId == tenantId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var trimmed = search.Trim();
            if (int.TryParse(trimmed, out _))
            {
                query = query.Where(op => op.Cedula.ToString().Contains(trimmed));
            }
            else
            {
                var lowered = trimmed.ToLower();
                query = query.Where(op => (op.Persona.Nombre + " " + op.Persona.Apellido).ToLower().Contains(lowered));
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(op => op.Persona.Apellido).ThenBy(op => op.Persona.Nombre)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(op => new AsistenciaElectorDto(
                op.UserId,
                op.Cedula,
                (op.Persona.Apellido + " " + op.Persona.Nombre).Trim(),
                op.NroTelefono,
                op.Persona.Inscripciones.OrderByDescending(i => i.Id)
                    .Select(i => i.Localidad != null ? i.Localidad.Descrip : null)
                    .FirstOrDefault(),
                op.User != null ? op.User.FullName : null,
                op.Asistio,
                op.AsistioMarcadoEn
            ))
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<AsistenciaResumenDto> GetAsistenciaResumenAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        var query = _context.OperadorPersonas.Where(op => op.TenantId == tenantId);

        var total = await query.CountAsync(cancellationToken);
        var asistieron = await query.CountAsync(op => op.Asistio, cancellationToken);
        var faltantes = total - asistieron;
        var porcentaje = total == 0 ? 0 : Math.Round(asistieron * 100.0 / total, 1);

        return new AsistenciaResumenDto(total, asistieron, faltantes, porcentaje);
    }
}
