using Application.Common.Interfaces;
using Domain.Entities;

namespace Api.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private int? _manualTenantId;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetCurrentTenantId()
    {
        if (_manualTenantId.HasValue)
            return _manualTenantId.Value;

        var tenant = _httpContextAccessor.HttpContext?.Items["Tenant"] as Tenant;
        if (tenant is null)
            throw new UnauthorizedAccessException("Tenant not resolved.");

        return tenant.Id;
    }

    public void SetTenantId(int tenantId) {
        _manualTenantId = tenantId;
    }
}