using Application.Operadores.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UbicacionRepository : IUbicacionRepository
{
    private readonly AppDbContext _context;

    public UbicacionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Ubicacion ubicacion, CancellationToken cancellationToken = default)
    {
        await _context.Ubicaciones.AddAsync(ubicacion, cancellationToken);
    }
}
