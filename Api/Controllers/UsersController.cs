using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Application.Users.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly RegisterUser _registerUser;
    private readonly GetOperators _getOperators;
    private readonly GetCoordinators _getCoordinators;

    public UsersController(RegisterUser registerUser, ICurrentUserService currentUserService,
        GetOperators getOperators, GetCoordinators getCoordinators)
    {
        _registerUser = registerUser;
        _currentUserService = currentUserService;
        _getOperators = getOperators;
        _getCoordinators = getCoordinators;
    }

    [HttpPost]
    [RequiresPermission(Permissions.Users.CreateOperator)]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _registerUser.ExecuteAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [RequiresPermission(Permissions.Users.Read)]
    public async Task<IActionResult> GetOperators([FromQuery] PaginationQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _getOperators.ExecuteAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("coordinators")]
    [RequiresPermission(Permissions.Users.CreateOperator)]
    public async Task<IActionResult> GetCoordinators(CancellationToken cancellationToken)
    {
        var result = await _getCoordinators.ExecuteAsync(cancellationToken);
        return Ok(result);
    }
}
