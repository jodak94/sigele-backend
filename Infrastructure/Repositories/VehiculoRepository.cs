using Application.Vehiculos.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VehiculoRepository : IVehiculoRepository
{
    private readonly AppDbContext _context;

    public VehiculoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Vehiculo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Vehiculos
            .Include(v => v.Operador)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<Vehiculo> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Vehiculos.Include(v => v.Operador).AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(v => v.NombreDueno)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Vehiculo vehiculo, CancellationToken cancellationToken = default)
    {
        await _context.Vehiculos.AddAsync(vehiculo, cancellationToken);
    }

    public void Update(Vehiculo vehiculo)
    {
        _context.Vehiculos.Update(vehiculo);
    }

    public async Task<IEnumerable<Vehiculo>> GetAllForReporteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Vehiculos
            .Include(v => v.Operador)
            .OrderBy(v => v.NombreDueno)
            .ToListAsync(cancellationToken);
    }
}
