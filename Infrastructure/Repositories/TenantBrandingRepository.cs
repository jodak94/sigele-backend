using Application.Tenants.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TenantBrandingRepository : ITenantBrandingRepository
{
    private readonly AppDbContext _context;

    public TenantBrandingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TenantBranding?> GetByTenantIdAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.TenantBrandings
            .AsNoTracking()
            .Where(tb => tb.TenantId == tenantId && tb.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
