using Application.Users.Interfaces;

namespace Infrastructure.Security;
using BC = BCrypt.Net.BCrypt;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BC.HashPassword(password);
    public bool Verify(string password, string hash) => BC.Verify(password, hash);
}