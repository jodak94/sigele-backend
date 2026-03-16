using Application.Auth.Interfaces;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Application.Users.Interfaces;
using Domain.Entities;

namespace Application.Auth.UseCases;

public class Login
{
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly ITenantService _tenantService;

    public Login(IAuthRepository authRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService, ITenantService tenantService)
    {
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;;
        _tenantService = tenantService;
    }

    public async Task<AuthResponseDto> ExecuteAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        _tenantService.SetTenantId(dto.TenantId);
        var user = await _authRepository.GetUserByEmailAsync(dto.Email, cancellationToken);
        if (user is null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var permissions = user.Role.Permissions.Select(p => p.Name).ToList();
        var accessToken = _jwtService.GenerateToken(user, permissions);
        var refreshToken = new RefreshToken
        {
            Token = _jwtService.GenerateRefreshToken(),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)//TODO: Read from appsettings
        };

        await _authRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        await _authRepository.UpdateLastLoginAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddHours(8), //TODO: Read from appsettings
            User = new UserSessionDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.Name,
                Permissions = permissions
            }
        };
    }
}