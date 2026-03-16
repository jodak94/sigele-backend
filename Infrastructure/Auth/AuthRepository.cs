using Application.Auth.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken), cancellationToken);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    public Task RevokeRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        refreshToken.RevokedAt = DateTime.UtcNow;
        _context.RefreshTokens.Update(refreshToken);
        return Task.CompletedTask;
    }

    public Task UpdateLastLoginAsync(User user, CancellationToken cancellationToken = default)
    {
        user.LastLogin = DateTime.UtcNow;
        _context.Users.Update(user);
        return Task.CompletedTask;
    }
}