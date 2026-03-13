using Application.Users.DTOs;
using Application.Users.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly RegisterUser _registerUser;
    
    public UsersController(RegisterUser registerUser)
    {
        _registerUser = registerUser;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        var result = await _registerUser.ExecuteAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
    }
}