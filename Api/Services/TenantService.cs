using Application.Common.Interfaces;

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
        
        var claim = _httpContextAccessor.HttpContext?.User
            .FindFirst("tenantId");

        if (claim is null || !int.TryParse(claim.Value, out var tenantId))
            throw new UnauthorizedAccessException("Tenant not found in token.");

        return tenantId;
    }
    
    public void SetTenantId(int tenantId) {
        _manualTenantId = tenantId;
    }
}