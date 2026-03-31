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
    private readonly GetInfoDeOperadores _getInfoDeOperadores;
    private readonly BuscarElectorAsignado _buscarElectorAsignado;
    private readonly ActualizarElector _actualizarElector;
    private readonly RemoverElector _removerElector;
    private readonly GetElectorUbicaciones _getElectorUbicaciones;
    private readonly ICurrentUserService _currentUserService;

    public OperadoresController(
        AsignarElector asignarElector,
        GetElectoresDeOperador getElectoresDeOperador,
        GetInfoDeOperadores getInfoDeOperadores,
        BuscarElectorAsignado buscarElectorAsignado,
        ActualizarElector actualizarElector,
        RemoverElector removerElector,
        GetElectorUbicaciones getElectorUbicaciones,
        ICurrentUserService currentUserService)
    {
        _asignarElector = asignarElector;
        _getElectoresDeOperador = getElectoresDeOperador;
        _getInfoDeOperadores = getInfoDeOperadores;
        _buscarElectorAsignado = buscarElectorAsignado;
        _actualizarElector = actualizarElector;
        _removerElector = removerElector;
        _getElectorUbicaciones = getElectorUbicaciones;
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

    [HttpGet("info")]
    public async Task<IActionResult> GetInfoDeOperadores(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getInfoDeOperadores.ExecuteAsync(cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("electores/buscar")]
    public async Task<IActionResult> BuscarElectorAsignado([FromQuery] int cedula, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _buscarElectorAsignado.ExecuteAsync(cedula, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
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

    [HttpGet("electores/ubicaciones")]
    public async Task<IActionResult> GetElectorUbicaciones(CancellationToken cancellationToken)
    {
        var result = await _getElectorUbicaciones.ExecuteAsync(cancellationToken);
        return Ok(result);
    }
}
