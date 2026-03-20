using Application.Electores.UseCases;
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
        var result = await _getElectorByNumeroCed.ExecuteAsync(numeroCed, cancellationToken);

        if (!result.Any())
            return NotFound();

        return Ok(result);
    }
}
