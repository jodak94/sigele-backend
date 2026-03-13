using Domain.Entities;

namespace Application.Users.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id,  CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}