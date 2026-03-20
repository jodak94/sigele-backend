using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Electores.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/estadisticas")]
public class EstadisticasController : ControllerBase
{
    private readonly GetEstadisticasConsulta _getEstadisticas;

    public EstadisticasController(GetEstadisticasConsulta getEstadisticas)
    {
        _getEstadisticas = getEstadisticas;
    }

    [HttpGet("consultas")]
    [RequiresPermission(Permissions.Consultas.Read)]
    public async Task<IActionResult> GetConsultas(CancellationToken cancellationToken)
    {
        var tenant = (Tenant)HttpContext.Items["Tenant"]!;
        var result = await _getEstadisticas.ExecuteAsync(tenant.Id, cancellationToken);
        return Ok(result);
    }
}
