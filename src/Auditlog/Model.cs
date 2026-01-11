namespace InvestmentClubAPI.src.AuditLog;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid(); 
    public Guid? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public Guid? EntityId { get; set; } 
    public string Details { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
