namespace Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreatetAt { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
    public bool IsActive { get; set; } = true;
}