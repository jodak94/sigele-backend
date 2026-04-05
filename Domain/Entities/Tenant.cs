namespace Domain.Entities;

public class Tenant
{
    public int Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public bool IsActive { get; set; } = true;
    public bool SoportaUbicacion { get; set; } = false;
    public DateTime? OnboardingUntil { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? ElectorLimit { get; set; }
    public int ElectorCount { get; set; }
    public DateTime? AccessExpiresAt { get; set; }

    public bool EsPlanFull => ElectorLimit is null;
    public int? GraceLimit => ElectorLimit.HasValue ? ElectorLimit + 50 : null;
    public bool CaptacionBloqueada => !EsPlanFull && ElectorCount >= GraceLimit;
    public bool EnGracia => !EsPlanFull && ElectorCount >= ElectorLimit && ElectorCount < GraceLimit;
}