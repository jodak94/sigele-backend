namespace Domain.Entities;

public class Tenant
{
    public int Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
}