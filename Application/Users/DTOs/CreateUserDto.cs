namespace Application.Users.DTOs;

public class CreateUserDto
{
    public string FullName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Phone { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public int RoleId { get; set; }
}