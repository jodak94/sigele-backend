using Application.Padron;
using Domain.Entities.Padron;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PersonaRepository : IPersonaRepository
{
    private readonly AppDbContext _context;

    public PersonaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Persona?> GetByCedulaAsync(int cedula, CancellationToken cancellationToken = default)
    {
        return await _context.Personas.FindAsync([cedula], cancellationToken);
    }
}
