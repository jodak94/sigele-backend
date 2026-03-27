using static Application.Common.Constants.Roles;
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
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<(IEnumerable<User> Items, int TotalCount)> GetOperatorsAsync(int? coordinatorId, int page, int pageSize, string? nombre, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.Include(u => u.Role).Where(u => u.Role.Name == Operator);

        if (coordinatorId.HasValue)
            query = query.Where(u => u.CoordinatorId == coordinatorId.Value);

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(u => u.FullName.Contains(nombre));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(u => u.CoordinatorId)
            .ThenBy(u => u.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IEnumerable<User>> GetCoordinatorsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.Name == Coordinator)
            .OrderBy(u => u.FullName)
            .ToListAsync(cancellationToken);
    }
}
