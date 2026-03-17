using Application.Users.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FindAsync([id], cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>  u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<(IEnumerable<User> Items, int TotalCount)> GetOperatorsAsync(int? createdBy, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.Include(u => u.Role).Where(u => u.Role.Name == "Operador");

        if (createdBy.HasValue)
            query = query.Where(u => u.CreatedBy == createdBy.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query.
            OrderBy(u => u.CreatedBy).
            ThenBy(u => u.FullName).
            Skip((page -1) * pageSize).
            Take(pageSize).
            ToListAsync(cancellationToken);
        
        return (items, totalCount);
    }
}