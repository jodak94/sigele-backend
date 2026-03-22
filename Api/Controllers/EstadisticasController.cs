using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Electores.UseCases;
using Application.Operadores.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/estadisticas")]
public class EstadisticasController : ControllerBase
{
    private readonly GetEstadisticasConsulta _getEstadisticas;
    private readonly GetEstadisticasOperadores _getEstadisticasOperadores;

    public EstadisticasController(
        GetEstadisticasConsulta getEstadisticas,
        GetEstadisticasOperadores getEstadisticasOperadores)
    {
        _getEstadisticas = getEstadisticas;
        _getEstadisticasOperadores = getEstadisticasOperadores;
    }

    [HttpGet("consultas")]
    [RequiresPermission(Permissions.Consultas.Read)]
    public async Task<IActionResult> GetConsultas(CancellationToken cancellationToken)
    {
        var result = await _getEstadisticas.ExecuteAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("operadores")]
    [Authorize]
    public async Task<IActionResult> GetOperadores(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getEstadisticasOperadores.ExecuteAsync(cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
