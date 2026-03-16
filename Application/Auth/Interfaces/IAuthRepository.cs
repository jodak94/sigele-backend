using Domain.Entities;

namespace Application.Auth.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task RevokeRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task UpdateLastLoginAsync(User user, CancellationToken cancellationToken = default);
}