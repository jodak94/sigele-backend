using Domain.Entities;

namespace Application.Users.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
}