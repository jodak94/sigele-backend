namespace Application.Users.DTOs;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } =  string.Empty;
    public int TenantId { get; set; }
}