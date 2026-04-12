using Application.Common.Constants;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OperadorElectorRepository : IOperadorElectorRepository
{
    private readonly AppDbContext _context;

    public OperadorElectorRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> ElectorActivoEnTenantAsync(int electorId, int tenantId, CancellationToken cancellationToken = default)
    {
        return _context.OperadorElectores
            .AnyAsync(oe => oe.ElectorId == electorId && oe.TenantId == tenantId && oe.IsActive, cancellationToken);
    }

    public async Task AddAsync(OperadorElector operadorElector, CancellationToken cancellationToken = default)
    {
        await _context.OperadorElectores.AddAsync(operadorElector, cancellationToken);
    }

    public Task<OperadorElector?> GetAsync(int operadorId, int electorId, CancellationToken cancellationToken = default)
    {
        return _context.OperadorElectores
            .Include(oe => oe.Ubicacion)
            .FirstOrDefaultAsync(oe => oe.UserId == operadorId && oe.ElectorId == electorId, cancellationToken);
    }

    public async Task<IEnumerable<OperadorElectorDto>> GetByOperadorAsync(int operadorId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.UserId == operadorId && oe.IsActive)
            .Select(oe => new OperadorElectorDto(
                oe.ElectorId,
                oe.Elector.Nombre,
                oe.Elector.Apellido,
                oe.Elector.NumeroCed,
                oe.DisponibleMiembroMesa,
                oe.RequiereTransporte,
                oe.NroTelefono,
                oe.DireccionRecogida,
                oe.Elector.Local != null ? oe.Elector.Local.NombreLoc : null,
                oe.Elector.Mesa,
                oe.Elector.Orden,
                oe.Ubicacion != null
                    ? new UbicacionDto(oe.Ubicacion.Lat, oe.Ubicacion.Lng, oe.Ubicacion.Descripcion)
                    : null
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ElectorAsignadoDto>> BuscarPorNumeroCedAsync(int numeroCed, int? operatorId, int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        var query = _context.OperadorElectores
            .Where(oe => oe.IsActive && oe.TenantId == tenantId && oe.Elector.NumeroCed == numeroCed);

        if (operatorId.HasValue)
            query = query.Where(oe => oe.UserId == operatorId.Value);
        else if (coordinatorId.HasValue)
            query = query.Where(oe => oe.User.CoordinatorId == coordinatorId.Value);

        return await query
            .Select(oe => new ElectorAsignadoDto(
                oe.ElectorId,
                oe.Elector.Nombre,
                oe.Elector.Apellido,
                oe.Elector.NumeroCed,
                oe.DisponibleMiembroMesa,
                oe.RequiereTransporte,
                oe.NroTelefono,
                oe.DireccionRecogida,
                new OperadorBasicoDto(oe.UserId, oe.User.FullName, oe.User.Email, oe.User.Phone)
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
                _context.OperadorElectores.Count(oe => oe.UserId == u.Id && oe.IsActive &&
                    (!codigoSeccional.HasValue || oe.Elector.CodigoSec == codigoSeccional.Value)),
                _context.OperadorElectores.Count(oe => oe.UserId == u.Id && oe.IsActive && oe.DisponibleMiembroMesa &&
                    (!codigoSeccional.HasValue || oe.Elector.CodigoSec == codigoSeccional.Value)),
                _context.OperadorElectores.Count(oe => oe.UserId == u.Id && oe.IsActive && oe.RequiereTransporte &&
                    (!codigoSeccional.HasValue || oe.Elector.CodigoSec == codigoSeccional.Value))
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CandidatoMesaFlatItemDto>> GetCandidatosMesaFlatAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId &&
                         oe.DisponibleMiembroMesa &&
                         (!coordinatorId.HasValue || oe.User.CoordinatorId == coordinatorId.Value))
            .Select(oe => new CandidatoMesaFlatItemDto(
                oe.Elector.Local != null ? oe.Elector.Local.NombreLoc : null,
                (oe.Elector.Apellido + " " + oe.Elector.Nombre).Trim(),
                oe.Elector.NumeroCed,
                oe.NroTelefono,
                oe.Elector.Mesa,
                oe.User.FullName
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DiaDFlatItemDto>> GetDiaDFlatAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId &&
                         (!coordinatorId.HasValue || oe.User.CoordinatorId == coordinatorId.Value))
            .Select(oe => new DiaDFlatItemDto(
                oe.Elector.Local != null ? oe.Elector.Local.NombreLoc : null,
                oe.Elector.Mesa,
                oe.Elector.Orden,
                oe.Elector.NumeroCed,
                (oe.Elector.Apellido + " " + oe.Elector.Nombre).Trim(),
                oe.NroTelefono,
                oe.DireccionRecogida,
                oe.RequiereTransporte,
                oe.User.FullName
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
                _context.OperadorElectores.Count(oe => oe.UserId == u.Id && oe.IsActive),
                _context.OperadorElectores.Count(oe => oe.UserId == u.Id && oe.IsActive && oe.DisponibleMiembroMesa),
                _context.OperadorElectores.Count(oe => oe.UserId == u.Id && oe.IsActive && oe.RequiereTransporte)
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
                _context.OperadorElectores.Count(oe => oe.User.CoordinatorId == u.Id && oe.IsActive),
                _context.OperadorElectores.Count(oe => oe.User.CoordinatorId == u.Id && oe.IsActive && oe.DisponibleMiembroMesa)
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<OperadorElector?> GetByUserAndElectorAsync(
        int userId,
        int electorId,
        bool includeInactive,
        CancellationToken cancellationToken = default)
    {
        IQueryable<OperadorElector> query = _context.OperadorElectores;

        if (includeInactive)
            query = query.IgnoreQueryFilters();

        return await query
            .FirstOrDefaultAsync(
                oe => oe.UserId == userId && oe.ElectorId == electorId,
                cancellationToken
            );
    }

    public async Task<IEnumerable<ElectorUbicacionDto>> GetElectorUbicacionesAsync(
        int? operadorId,
        int? coordinadorId,
        int tenantId,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OperadorElectores
            .Where(oe => oe.TenantId == tenantId && oe.UbicacionId != null);

        if (operadorId.HasValue)
            query = query.Where(oe => oe.UserId == operadorId.Value);
        else if (coordinadorId.HasValue)
            query = query.Where(oe => oe.User.CoordinatorId == coordinadorId.Value);

        return await query
            .Select(oe => new ElectorUbicacionDto(
                oe.ElectorId,
                oe.Elector.Nombre,
                oe.Elector.Apellido,
                oe.Elector.NumeroCed,
                oe.Ubicacion!.Lat,
                oe.Ubicacion!.Lng,
                oe.Ubicacion!.Descripcion
            ))
            .ToListAsync(cancellationToken);
    }
}
