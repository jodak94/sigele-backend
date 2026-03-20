using Application.Tenants.Interfaces;
using Domain.Entities;

namespace Api.Middleware;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantRepository tenantRepository)
    {
        var origin = context.Request.Headers.Origin.ToString();

        if (string.IsNullOrWhiteSpace(origin) || !Uri.TryCreate(origin, UriKind.Absolute, out var originUri))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Header 'Origin' ausente o inválido.");
            return;
        }

        var subdomain = ExtractSubdomain(originUri.Host); // e.g. "naomyferrer" de "naomyferrer.sigele.com.py"

        if (string.IsNullOrWhiteSpace(subdomain))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Subdominio no identificado.");
            return;
        }

        var tenant = await tenantRepository.GetBySubdomainAsync(subdomain);

        if (tenant is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Tenant no autorizado.");
            return;
        }

        context.Items["Tenant"] = tenant;
        await _next(context);
    }

    private static string? ExtractSubdomain(string host)
    {
        var parts = host.Split('.');
        return parts.Length > 1 ? parts[0] : null;
    }
}
