using Application.Tenants.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _context;

    public TenantRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Tenant?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<Tenant?> GetByDomainAsync(string domain, CancellationToken cancellationToken = default)
    {
        return _context.Tenants
            .Where(t => t.IsActive && t.Domain == domain)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Tenant?> GetBySubdomainAsync(string subdomain, CancellationToken cancellationToken = default)
    {
        return _context.Tenants
            .Where(t => t.IsActive && t.Subdomain == subdomain)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<TenantPackage>> GetPackagesByTenantIdAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        return _context.TenantPackages
            .Where(p => p.TenantId == tenantId)
            .OrderByDescending(p => p.PurchasedAt)
            .ToListAsync(cancellationToken);
    }

    public Task IncrementElectorCountAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        return _context.Tenants
            .Where(t => t.Id == tenantId)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.ElectorCount, t => t.ElectorCount + 1), cancellationToken);
    }

    public Task DecrementElectorCountAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        return _context.Tenants
            .Where(t => t.Id == tenantId)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.ElectorCount, t => t.ElectorCount - 1), cancellationToken);
    }

}
