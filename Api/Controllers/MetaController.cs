using Application.Tenants.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class MetaController : ControllerBase
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantBrandingRepository _brandingRepository;

    public MetaController(ITenantRepository tenantRepository, ITenantBrandingRepository brandingRepository)
    {
        _tenantRepository = tenantRepository;
        _brandingRepository = brandingRepository;
    }

    [HttpGet("/api/meta/preview")]
    [AllowAnonymous] 
    public async Task<IActionResult> Preview()
    {
        var host = Request.Headers.Host.ToString(); // "naomyferrer.sigele.com.py"
        var subdomain = ExtractSubdomain(host);

        if (string.IsNullOrWhiteSpace(subdomain))
            return Content(FallbackHtml(), "text/html");

        var tenant = await _tenantRepository.GetBySubdomainAsync(subdomain);
        if (tenant is null)
            return Content(FallbackHtml(), "text/html");

        var branding = await _brandingRepository.GetByTenantIdAsync(tenant.Id);
        if (branding is null)
            return Content(FallbackHtml(), "text/html");

        return Content(BuildHtml(branding, host), "text/html");
    }

    private static string ExtractSubdomain(string host)
    {
        // "naomyferrer.sigele.com.py" → "naomyferrer"
        var parts = host.Split('.');
        return parts.Length >= 4 ? parts[0] : string.Empty;
    }

    private static string BuildHtml(TenantBranding branding, string host)
    {
        var url = $"https://{host}";
        return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8"/>
                <title>{branding.AppTitle}</title>
                <meta property="og:title" content="{branding.AppTitle}" />
                <meta property="og:description" content="{branding.AppTitle} — Sigele" />
                <meta property="og:image" content="{branding.FaviconUrl}" />
                <meta property="og:url" content="{url}" />
                <meta property="og:type" content="website" />
                <meta name="twitter:card" content="summary" />
                <meta name="twitter:title" content="{branding.AppTitle}" />
                <meta name="twitter:image" content="{branding.FaviconUrl}" />
                <meta http-equiv="refresh" content="0;url={url}" />
            </head>
            <body></body>
            </html>
            """;
    }

    private static string FallbackHtml() => """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="utf-8"/>
            <title>SIGELE</title>
            <meta property="og:title" content="SIGELE — Gestión Electoral Digital" />
            <meta http-equiv="refresh" content="0;url=https://sigele.com.py" />
        </head>
        <body></body>
        </html>
        """;
}