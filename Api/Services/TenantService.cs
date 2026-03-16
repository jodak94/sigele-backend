using Application.Common.Interfaces;

namespace Api.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetCurrentTenantId()
    {
        var claim = _httpContextAccessor.HttpContext?.User
            .FindFirst("tenant_id");

        if (claim is null || !int.TryParse(claim.Value, out var tenantId))
            throw new UnauthorizedAccessException("Tenant not found in token.");

        return tenantId;
    }
}