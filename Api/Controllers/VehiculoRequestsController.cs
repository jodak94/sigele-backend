using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.DTOs;
using Application.VehiculoRequests.DTOs;
using Application.VehiculoRequests.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/vehiculo-requests")]
[Authorize]
public class VehiculoRequestsController : ControllerBase
{
    private readonly GetVehiculoRequests _getVehiculoRequests;
    private readonly AprobarVehiculoRequest _aprobarVehiculoRequest;
    private readonly RechazarVehiculoRequest _rechazarVehiculoRequest;

    public VehiculoRequestsController(
        GetVehiculoRequests getVehiculoRequests,
        AprobarVehiculoRequest aprobarVehiculoRequest,
        RechazarVehiculoRequest rechazarVehiculoRequest)
    {
        _getVehiculoRequests = getVehiculoRequests;
        _aprobarVehiculoRequest = aprobarVehiculoRequest;
        _rechazarVehiculoRequest = rechazarVehiculoRequest;
    }

    [HttpGet]
    [RequiresPermission(Permissions.Vehiculos.Read)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _getVehiculoRequests.ExecuteAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/aprobar")]
    [RequiresPermission(Permissions.Vehiculos.Create)]
    public async Task<IActionResult> Aprobar(int id, [FromBody] ResolveVehiculoRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _aprobarVehiculoRequest.ExecuteAsync(id, dto, cancellationToken);
            return NoContent();
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

    [HttpPost("{id:int}/rechazar")]
    [RequiresPermission(Permissions.Vehiculos.Create)]
    public async Task<IActionResult> Rechazar(int id, [FromBody] ResolveVehiculoRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _rechazarVehiculoRequest.ExecuteAsync(id, dto, cancellationToken);
            return NoContent();
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
}
