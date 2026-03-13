using Domain.Entities;

namespace Application.Users.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } =  string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? LastLogin { get; set; }
    public RoleDto Role { get; set; } = null!;
}

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<string> Permissions { get; set; } = new List<string>();
}