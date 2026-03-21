using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/operadores")]
[Authorize]
public class OperadoresController : ControllerBase
{
    private readonly AsignarElector _asignarElector;
    private readonly GetElectoresDeOperador _getElectoresDeOperador;
    private readonly ActualizarElector _actualizarElector;
    private readonly RemoverElector _removerElector;
    private readonly ICurrentUserService _currentUserService;

    public OperadoresController(
        AsignarElector asignarElector,
        GetElectoresDeOperador getElectoresDeOperador,
        ActualizarElector actualizarElector,
        RemoverElector removerElector,
        ICurrentUserService currentUserService)
    {
        _asignarElector = asignarElector;
        _getElectoresDeOperador = getElectoresDeOperador;
        _actualizarElector = actualizarElector;
        _removerElector = removerElector;
        _currentUserService = currentUserService;
    }

    [HttpPost("electores")]
    public async Task<IActionResult> AsignarElector([FromBody] AsignarElectorDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _asignarElector.ExecuteAsync(_currentUserService.UserId, dto, cancellationToken);
            return Created(string.Empty, null);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("electores/{electorId:int}")]
    public async Task<IActionResult> ActualizarElector(int electorId, [FromBody] ActualizarElectorDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _actualizarElector.ExecuteAsync(electorId, dto, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("electores/{electorId:int}")]
    public async Task<IActionResult> RemoverElector(int electorId, CancellationToken cancellationToken)
    {
        try
        {
            await _removerElector.ExecuteAsync(electorId, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{operadorId:int}/electores")]
    public async Task<IActionResult> GetElectores(int operadorId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getElectoresDeOperador.ExecuteAsync(operadorId, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
