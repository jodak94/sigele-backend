using Domain.Entities;

namespace Api.Middleware;

public class TenantValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TenantValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = context.User.FindFirst("tenantId")?.Value;
            if (!int.TryParse(tenantClaim, out var jwtTenantId))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Tenant inválido en token.");
                return;
            }

            var resolvedTenant = context.Items["Tenant"] as Tenant;
            if (resolvedTenant is null || resolvedTenant.Id != jwtTenantId)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Token no corresponde al tenant de este dominio.");
                return;
            }
        }

        await _next(context);
    }
}
