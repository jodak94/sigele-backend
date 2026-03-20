using Domain.Common;

namespace Domain.Entities;

public class User : AuditableEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } =  string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } =  string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime? LastLogin { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public int? CoordinatorId { get; set; }
    public User? Coordinator { get; set; }
    public ICollection<User> Operators { get; set; } = new List<User>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}