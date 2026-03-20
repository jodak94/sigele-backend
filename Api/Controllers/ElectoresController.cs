using Application.Electores.DTOs;
using Application.Electores.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/electores")]
[AllowAnonymous]
public class ElectoresController : ControllerBase
{
    private readonly GetElectorByNumeroCed _getElectorByNumeroCed;

    public ElectoresController(GetElectorByNumeroCed getElectorByNumeroCed)
    {
        _getElectorByNumeroCed = getElectorByNumeroCed;
    }

    [HttpGet("{numeroCed:int}")]
    public async Task<IActionResult> GetByNumeroCed(int numeroCed, CancellationToken cancellationToken)
    {
        var tenant = (Tenant)HttpContext.Items["Tenant"]!;

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "desconocida";
        var contexto = new ConsultaContextDto(
            TenantId:   tenant.Id,
            IpCliente:  ip,
            UserAgent:  Request.Headers.UserAgent.ToString(),
            Origin:     Request.Headers.Origin.ToString(),
            Host:       Request.Host.Value,
            MetodoHttp: Request.Method);

        var result = await _getElectorByNumeroCed.ExecuteAsync(numeroCed, contexto, cancellationToken);

        if (!result.Any())
            return NotFound();

        return Ok(result);
    }
}
