using Application.Tenants.DTOs;
using Application.Tenants.Interfaces;

namespace Application.Tenants.UseCases;

public class GetTenantBranding
{
    private readonly ITenantBrandingRepository _brandingRepository;

    public GetTenantBranding(ITenantBrandingRepository brandingRepository)
    {
        _brandingRepository = brandingRepository;
    }

    public async Task<TenantBrandingDto?> ExecuteAsync(int tenantId, CancellationToken cancellationToken = default)
    {
        var branding = await _brandingRepository.GetByTenantIdAsync(tenantId, cancellationToken);

        if (branding is null)
            return null;

        return new TenantBrandingDto(
            branding.AppTitle,
            branding.PrimaryColor,
            branding.SecondaryColor,
            branding.FaviconUrl,
            branding.CandidateName,
            branding.CandidateTitle
        );
    }
}
