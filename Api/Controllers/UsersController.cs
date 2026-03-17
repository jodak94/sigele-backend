using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Application.Users.UseCases;
using Domain.Entities;
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
    
    public UsersController(RegisterUser registerUser,  ICurrentUserService currentUserService,  GetOperators getOperators)
    {
        _registerUser = registerUser;
        _currentUserService = currentUserService;
        _getOperators = getOperators;
    }

    [HttpPost]
    [RequiresPermission(Permissions.Users.Create)]//General 
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        // specific rule: coordinator can only assign operator
        //                admin can assign operator and coordinator
        if (dto.RoleId == 2 && !_currentUserService.HasPermission(Permissions.Users.CreateCoordinator))
            return Forbid();

        if (dto.RoleId == 1)
            return Forbid(); // nobody creates admin through this endpoint
        
        var result = await _registerUser.ExecuteAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
    }
    
    [HttpGet]
    [RequiresPermission(Permissions.Users.Read)]
    public async Task<IActionResult> GetOperators([FromQuery] PaginationQueryDto query, CancellationToken cancellationToken){
        var result = await _getOperators.ExecuteAsync(query, cancellationToken);
        return Ok(result);
    }
}