using Application.Auth.Interfaces;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Application.Users.Interfaces;
using Domain.Entities;

namespace Application.Auth.UseCases;

public class RefreshTokenUseCase
{
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public RefreshTokenUseCase(IAuthRepository authRepository, IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> ExecuteAsync(RefreshTokenRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var user = await _authRepository.GetUserByRefreshTokenAsync(dto.RefreshToken, cancellationToken);
        if(user == null)
            throw new UnauthorizedAccessException("Invalid Refresh Token");

        var existingToken = user.RefreshTokens.First(t => t.Token == dto.RefreshToken);
        if(!existingToken.IsActive)
            throw new UnauthorizedAccessException("Refresh Token has expired or been revoked");

        await _authRepository.RevokeRefreshTokenAsync(existingToken, cancellationToken);
        
        var permissions = user.Role.Permissions.Select(p => p.Name).ToList();
        var newAccessToken = _jwtService.GenerateToken(user, permissions);
        var newRefreshToken = new RefreshToken
        {
            Token = _jwtService.GenerateRefreshToken(),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _authRepository.AddRefreshTokenAsync(newRefreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddHours(8),
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