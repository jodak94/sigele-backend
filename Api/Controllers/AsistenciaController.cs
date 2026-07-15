using Application.Asistencia.DTOs;
using Application.Asistencia.UseCases;
using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/asistencia")]
[Authorize]
public class AsistenciaController : ControllerBase
{
    private readonly GetAsistenciaList _getAsistenciaList;
    private readonly GetAsistenciaResumen _getAsistenciaResumen;
    private readonly MarcarAsistencia _marcarAsistencia;

    public AsistenciaController(GetAsistenciaList getAsistenciaList, GetAsistenciaResumen getAsistenciaResumen,
        MarcarAsistencia marcarAsistencia)
    {
        _getAsistenciaList = getAsistenciaList;
        _getAsistenciaResumen = getAsistenciaResumen;
        _marcarAsistencia = marcarAsistencia;
    }

    [HttpGet]
    [RequiresPermission(Permissions.Asistencia.Read)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQueryDto query, [FromQuery] string? search, CancellationToken cancellationToken)
    {
        var result = await _getAsistenciaList.ExecuteAsync(query, search, cancellationToken);
        return Ok(result);
    }

    [HttpGet("resumen")]
    [RequiresPermission(Permissions.Asistencia.Read)]
    public async Task<IActionResult> GetResumen(CancellationToken cancellationToken)
    {
        var result = await _getAsistenciaResumen.ExecuteAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{userId:int}/{cedula:int}")]
    [RequiresPermission(Permissions.Asistencia.Update)]
    public async Task<IActionResult> MarcarAsistencia(int userId, int cedula, [FromBody] MarcarAsistenciaDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _marcarAsistencia.ExecuteAsync(userId, cedula, dto, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
