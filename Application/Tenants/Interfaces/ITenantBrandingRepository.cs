using Domain.Entities;

namespace Application.Tenants.Interfaces;

public interface ITenantBrandingRepository
{
    Task<TenantBranding?> GetByTenantIdAsync(int tenantId, CancellationToken cancellationToken = default);
}
