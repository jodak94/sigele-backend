using Application.Electores.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class ElectorConsultaRepository : IElectorConsultaRepository
{
    private readonly AppDbContext _context;

    public ElectorConsultaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(ElectorConsulta consulta, CancellationToken cancellationToken = default)
    {
        _context.ElectorConsultas.Add(consulta);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
