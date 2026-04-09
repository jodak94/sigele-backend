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
    private readonly GetUsers _getUsers;
    private readonly GetOperators _getOperators;
    private readonly GetCoordinators _getCoordinators;
    private readonly ResetPassword _resetPassword;
    private readonly AdminResetPassword _adminResetPassword;

    public UsersController(RegisterUser registerUser, ICurrentUserService currentUserService,
        GetUsers getUsers, GetOperators getOperators, GetCoordinators getCoordinators,
        ResetPassword resetPassword, AdminResetPassword adminResetPassword)
    {
        _registerUser = registerUser;
        _currentUserService = currentUserService;
        _getUsers = getUsers;
        _getOperators = getOperators;
        _getCoordinators = getCoordinators;
        _resetPassword = resetPassword;
        _adminResetPassword = adminResetPassword;
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

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers([FromQuery] PaginationQueryDto query, [FromQuery] string? nombre, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getUsers.ExecuteAsync(_currentUserService.Role, query, nombre, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet]
    [RequiresPermission(Permissions.Users.Read)]
    public async Task<IActionResult> GetOperators([FromQuery] PaginationQueryDto query, [FromQuery] string? nombre, CancellationToken cancellationToken)
    {
        var result = await _getOperators.ExecuteAsync(query, nombre, cancellationToken);
        return Ok(result);
    }

    [HttpGet("coordinators")]
    [RequiresPermission(Permissions.Users.CreateOperator)]
    public async Task<IActionResult> GetCoordinators(CancellationToken cancellationToken)
    {
        var result = await _getCoordinators.ExecuteAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/admin-reset-password")]
    public async Task<IActionResult> AdminResetPassword(int id, [FromBody] AdminResetPasswordDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _adminResetPassword.ExecuteAsync(_currentUserService.Role, id, dto, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id:int}/password")]
    public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _resetPassword.ExecuteAsync(_currentUserService.UserId, _currentUserService.Role, id, dto, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
