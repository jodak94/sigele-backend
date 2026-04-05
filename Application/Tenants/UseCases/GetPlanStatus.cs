using Application.Common.Interfaces;
using Application.Tenants.DTOs;
using Application.Tenants.Interfaces;
using Domain.Common;

namespace Application.Tenants.UseCases;

public class GetPlanStatus
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetPlanStatus(ITenantRepository tenantRepository, ICurrentUserService currentUserService)
    {
        _tenantRepository = tenantRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PlanStatusDto> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = _currentUserService.TenantId;

        var tenant = await _tenantRepository.GetByIdAsync(tenantId, cancellationToken);
        if (tenant is null)
            throw new KeyNotFoundException("Tenant no encontrado.");

        var paquetes = await _tenantRepository.GetPackagesByTenantIdAsync(tenantId, cancellationToken);

        var porcentajeUso = tenant.EsPlanFull || tenant.ElectorLimit is null or 0
            ? 0m
            : Math.Min(100m, Math.Round((decimal)tenant.ElectorCount / tenant.ElectorLimit.Value * 100, 2));

        var accesoVencido = tenant.AccessExpiresAt.HasValue && tenant.AccessExpiresAt.Value < DateTime.UtcNow;

        return new PlanStatusDto
        {
            ElectorCount       = tenant.ElectorCount,
            ElectorLimit       = tenant.ElectorLimit,
            EsPlanFull         = tenant.EsPlanFull,
            EnGracia           = tenant.EnGracia,
            CaptacionBloqueada = tenant.CaptacionBloqueada,
            PorcentajeUso      = porcentajeUso,
            AccessExpiresAt    = tenant.AccessExpiresAt,
            AccesoVencido      = accesoVencido,
            Paquetes           = paquetes.Select(p => new PaqueteDto
            {
                PackageType        = p.PackageType,
                Label              = PackageTypes.Labels.GetValueOrDefault(p.PackageType, p.PackageType),
                ElectoresAgregados = p.ElectoresAgregados,
                Precio             = p.Precio,
                PurchasedAt        = p.PurchasedAt,
            }).ToList(),
        };
    }
}
