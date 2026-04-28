using System.Diagnostics;
using Application.Electores.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ElectorRepository : IElectorRepository
{
    private readonly AppDbContext _context;

    public ElectorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Elector?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Electores.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<(Elector Elector, Seccional? Seccional)>> GetByNumeroCedAsync(int numeroCed, CancellationToken cancellationToken = default)
    {
        var electores = await _context.Electores
            .Include(e => e.Local)
            .Where(e => e.NumeroCed == numeroCed)
            .ToListAsync(cancellationToken);
        var sw = Stopwatch.StartNew();
        var results = new List<(Elector, Seccional?)>();
        sw.Stop();
        Console.WriteLine($"Query tardó: {sw.ElapsedMilliseconds}ms");
        foreach (var elector in electores)
        {
            Seccional? seccional = null;
            if (elector.CodDpto.HasValue && elector.CodDist.HasValue && elector.CodigoSec.HasValue)
            {
                seccional = await _context.Seccionales.FindAsync(
                    [elector.CodDpto.Value, elector.CodDist.Value, elector.CodigoSec.Value],
                    cancellationToken);
            }
            results.Add((elector, seccional));
        }

        return results;
    }
}
