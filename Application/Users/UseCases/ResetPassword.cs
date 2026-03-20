using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Application.Users.Interfaces;

namespace Application.Users.UseCases;

public class ResetPassword
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPassword(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    /// <param name="requesterId">ID del usuario que hace la petición.</param>
    /// <param name="requesterRole">Rol del usuario que hace la petición.</param>
    /// <param name="targetUserId">ID del usuario cuya contraseña se cambia.</param>
    public async Task ExecuteAsync(int requesterId, string requesterRole, int targetUserId, ResetPasswordDto dto, CancellationToken cancellationToken = default)
    {
        var isSelf = requesterId == targetUserId;
        var isAdmin = requesterRole == Admin;

        if (!isSelf && !isAdmin)
            throw new UnauthorizedAccessException("No tiene permiso para cambiar la contraseña de otro usuario.");

        var target = await _userRepository.GetByIdAsync(targetUserId, cancellationToken)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        if (!_passwordHasher.Verify(dto.CurrentPassword, target.PasswordHash))
            throw new InvalidOperationException("La contraseña actual es incorrecta.");

        target.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
        target.MustChangePassword = false;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
