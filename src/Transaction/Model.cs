namespace InvestmentClubAPI.src.Transaction;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Direction { get; set; } = "In";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
