using Application.VehiculoRequests.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VehiculoRequestRepository : IVehiculoRequestRepository
{
    private readonly AppDbContext _context;

    public VehiculoRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VehiculoRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.VehiculoRequests
            .Include(v => v.Operador)
            .Include(v => v.AprobadoPor)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<VehiculoRequest> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.VehiculoRequests
            .Include(v => v.Operador)
            .Include(v => v.AprobadoPor)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(v => v.CreatetAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(VehiculoRequest request, CancellationToken cancellationToken = default)
    {
        await _context.VehiculoRequests.AddAsync(request, cancellationToken);
    }

    public void Update(VehiculoRequest request)
    {
        _context.VehiculoRequests.Update(request);
    }
}
