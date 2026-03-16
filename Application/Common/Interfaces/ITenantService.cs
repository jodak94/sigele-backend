namespace Application.Common.Interfaces;

public interface ITenantService
{
    int GetCurrentTenantId();
    void SetTenantId(int tenantId);
}