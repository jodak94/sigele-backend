using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Application.Users.Interfaces;
namespace Application.Users.UseCases;

public class AdminResetPassword
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public AdminResetPassword(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string requesterRole, int targetUserId, AdminResetPasswordDto dto, CancellationToken cancellationToken = default)
    {
        if (requesterRole != Common.Constants.Roles.Admin)
            throw new UnauthorizedAccessException("Solo el administrador puede restablecer contraseñas de otros usuarios.");

        var target = await _userRepository.GetByIdAsync(targetUserId, cancellationToken)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        target.PasswordHash = _passwordHasher.Hash(dto.ProvisionalPassword);
        target.MustChangePassword = true;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
