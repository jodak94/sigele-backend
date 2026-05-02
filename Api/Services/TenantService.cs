using Application.Common.Interfaces;
using Domain.Entities;

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
        var tenant = _httpContextAccessor.HttpContext?.Items["Tenant"] as Tenant;
        if (tenant is null)
            throw new UnauthorizedAccessException("Tenant not resolved.");

        return tenant.Id;
    }

    public string GetCurrentTenantSubdomain()
    {
        var tenant = _httpContextAccessor.HttpContext?.Items["Tenant"] as Tenant;
        if (tenant is null)
            throw new UnauthorizedAccessException("Tenant not resolved.");

        return tenant.Subdomain;
    }
}