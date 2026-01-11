namespace InvestmentClubAPI.src.ClubWallet;

public class ClubWallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClubId { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
