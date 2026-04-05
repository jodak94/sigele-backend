using Application.Tenants.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/tenant")]
[Authorize]
public class TenantController : ControllerBase
{
    private readonly GetPlanStatus _getPlanStatus;

    public TenantController(GetPlanStatus getPlanStatus)
    {
        _getPlanStatus = getPlanStatus;
    }

    [HttpGet("plan")]
    public async Task<IActionResult> GetPlanStatus(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getPlanStatus.ExecuteAsync(cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
