using Application.Auditoria.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auditoria")]
[Authorize]
public class AuditoriaController : ControllerBase
{
    private readonly GetAlertas              _getAlertas;
    private readonly GetMapaOperador         _getMapaOperador;
    private readonly GetCaptacionesDeOperador _getCaptacionesDeOperador;

    public AuditoriaController(GetAlertas getAlertas, GetMapaOperador getMapaOperador, GetCaptacionesDeOperador getCaptacionesDeOperador)
    {
        _getAlertas               = getAlertas;
        _getMapaOperador          = getMapaOperador;
        _getCaptacionesDeOperador = getCaptacionesDeOperador;
    }

    [HttpGet("alertas")]
    public async Task<IActionResult> GetAlertas(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getAlertas.ExecuteAsync(cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("operador/{operadorId:int}/captaciones")]
    public async Task<IActionResult> GetCaptacionesDeOperador(int operadorId, [FromQuery] string tipo_alerta, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getCaptacionesDeOperador.ExecuteAsync(operadorId, tipo_alerta, cancellationToken);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("mapa")]
    public async Task<IActionResult> GetMapa([FromQuery] int operadorId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getMapaOperador.ExecuteAsync(operadorId, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
