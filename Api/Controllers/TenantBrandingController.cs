using Application.Tenants.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/branding")]
[AllowAnonymous]
public class TenantBrandingController : ControllerBase
{
    private readonly GetTenantBranding _getTenantBranding;

    public TenantBrandingController(GetTenantBranding getTenantBranding)
    {
        _getTenantBranding = getTenantBranding;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var tenant = (Tenant)HttpContext.Items["Tenant"]!;
        var branding = await _getTenantBranding.ExecuteAsync(tenant.Id, cancellationToken);

        if (branding is null)
            return NotFound();

        return Ok(branding);
    }
}
