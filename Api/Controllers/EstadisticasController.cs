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
    private static readonly HashSet<int> TopValidos = [5, 10, 25, 50];

    private readonly GetEstadisticasConsulta      _getEstadisticas;
    private readonly GetEstadisticasOperadores    _getEstadisticasOperadores;
    private readonly GetEstadisticasPadronPublico _getEstadisticasPadronPublico;
    private readonly GetTopLocalesConsultados     _getTopLocales;
    private readonly GetUltimasConsultas          _getUltimasConsultas;
    private readonly GetEstadisticasZonales       _getEstadisticasZonales;
    private readonly GetRankingOperadores         _getRankingOperadores;
    private readonly GetResumenCoordinadores      _getResumenCoordinadores;

    public EstadisticasController(
        GetEstadisticasConsulta getEstadisticas,
        GetEstadisticasOperadores getEstadisticasOperadores,
        GetEstadisticasPadronPublico getEstadisticasPadronPublico,
        GetTopLocalesConsultados getTopLocales,
        GetUltimasConsultas getUltimasConsultas,
        GetEstadisticasZonales getEstadisticasZonales,
        GetRankingOperadores getRankingOperadores,
        GetResumenCoordinadores getResumenCoordinadores)
    {
        _getEstadisticas              = getEstadisticas;
        _getEstadisticasOperadores    = getEstadisticasOperadores;
        _getEstadisticasPadronPublico = getEstadisticasPadronPublico;
        _getTopLocales                = getTopLocales;
        _getUltimasConsultas          = getUltimasConsultas;
        _getEstadisticasZonales       = getEstadisticasZonales;
        _getRankingOperadores         = getRankingOperadores;
        _getResumenCoordinadores      = getResumenCoordinadores;
    }

    [HttpGet("consultas")]
    [RequiresPermission(Permissions.Consultas.Read)]
    public async Task<IActionResult> GetConsultas(CancellationToken cancellationToken)
    {
        var result = await _getEstadisticas.ExecuteAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("padron-publico")]
    [RequiresPermission(Permissions.Consultas.Read)]
    public async Task<IActionResult> GetPadronPublico(CancellationToken cancellationToken)
    {
        var result = await _getEstadisticasPadronPublico.ExecuteAsync(cancellationToken);
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

    [HttpGet("top-locales")]
    [RequiresPermission(Permissions.Consultas.Read)]
    public async Task<IActionResult> GetTopLocales([FromQuery] int top, CancellationToken cancellationToken)
    {
        if (!TopValidos.Contains(top))
            return BadRequest($"El parámetro 'top' debe ser uno de: {string.Join(", ", TopValidos)}.");

        var result = await _getTopLocales.ExecuteAsync(top, cancellationToken);
        return Ok(result);
    }

    [HttpGet("ultimas-consultas")]
    [RequiresPermission(Permissions.Consultas.Read)]
    public async Task<IActionResult> GetUltimasConsultas([FromQuery] int top, CancellationToken cancellationToken)
    {
        if (!TopValidos.Contains(top))
            return BadRequest($"El parámetro 'top' debe ser uno de: {string.Join(", ", TopValidos)}.");

        var result = await _getUltimasConsultas.ExecuteAsync(top, cancellationToken);
        return Ok(result);
    }

    [HttpGet("resumen-coordinadores")]
    [Authorize]
    public async Task<IActionResult> GetResumenCoordinadores(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getResumenCoordinadores.ExecuteAsync(cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("ranking-operadores")]
    [Authorize]
    public async Task<IActionResult> GetRankingOperadores(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getRankingOperadores.ExecuteAsync(cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("zonales/resumen")]
    [Authorize]
    public async Task<IActionResult> GetResumenZonal(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getEstadisticasZonales.GetResumenAsync(cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpGet("zonales/seccionales")]
    [Authorize]
    public async Task<IActionResult> GetSecccionalesCaptacion(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getEstadisticasZonales.GetListaAsync(cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
