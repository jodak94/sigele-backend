namespace Application.Tenants.DTOs;

public record TenantBrandingDto(
    string AppTitle,
    string PrimaryColor,
    string? SecondaryColor,
    string? FaviconUrl,
    string? CandidateName,
    string? CandidateTitle
);
