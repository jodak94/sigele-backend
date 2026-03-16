using Application.Auth.UseCases;
using Application.Users.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly Login _login;
    private readonly RefreshTokenUseCase _refreshTokenUseCase;

    public AuthController(Login login, RefreshTokenUseCase refreshTokenUseCase)
    {
        _login =  login;
        _refreshTokenUseCase = refreshTokenUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _login.ExecuteAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto, CancellationToken cancellationToken)
    {
        var result = await _refreshTokenUseCase.ExecuteAsync(dto, cancellationToken);
        return Ok(result);
    }
}