namespace InvestmentClubAPI.src.ClubMember;

public class ClubMember
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ClubId { get; set; }
    public string Role { get; set; } = "Member";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}