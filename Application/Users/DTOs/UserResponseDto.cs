namespace Application.Users.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? LastLogin { get; set; }
}