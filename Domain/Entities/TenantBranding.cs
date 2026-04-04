namespace Domain.Entities;

public class TenantBranding
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    // Branding
    public string AppTitle { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = string.Empty;
    public string? SecondaryColor { get; set; }
    public string? FaviconUrl { get; set; }

    // Candidato/a
    public string? CandidateName { get; set; }
    public string? CandidateTitle { get; set; }

    // Zona
    public string? Zona { get; set; }

    // Metadata
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}