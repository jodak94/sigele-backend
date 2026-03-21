using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
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
                oe.DireccionRecogida
            ))
            .ToListAsync(cancellationToken);
    }
}
