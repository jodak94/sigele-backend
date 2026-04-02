namespace Application.Users.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserSessionDto User { get; set; } = null!;
    public TenantConfigDto TenantConfig { get; set; } = null!;
}

public class UserSessionDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool MustChangePassword { get; set; }
    public IEnumerable<string> Permissions { get; set; } = new List<string>();
}

public class TenantConfigDto
{
    public bool SoportaUbicacion { get; set; }
}