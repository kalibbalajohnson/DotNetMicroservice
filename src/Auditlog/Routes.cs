using InvestmentClubAPI.Data;

namespace InvestmentClubAPI.src.AuditLog;

public class AuditService
{
    private readonly AppDbContext _db;

    public AuditService(AppDbContext db)
    {
        _db = db;
    }

    public async Task LogAsync(
        Guid? userId,
        string action,
        string entity,
        Guid? entityId = null,
        string details = ""
    )
    {
        _db.AuditLogs.Add(new AuditLog
        {
            UserId = userId,
            Action = action,
            Entity = entity,
            EntityId = entityId,
            Details = details,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }
}