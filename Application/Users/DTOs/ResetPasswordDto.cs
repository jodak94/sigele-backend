namespace Application.Users.DTOs;

public record ResetPasswordDto(
    string CurrentPassword,
    string NewPassword
);
