using Microsoft.EntityFrameworkCore;
using InvestmentClubAPI.src.UserAuth;
using InvestmentClubAPI.src.AuditLog;
using InvestmentClubAPI.src.UserProfile;
using InvestmentClubAPI.src.Club;
using InvestmentClubAPI.src.ClubMember;
using InvestmentClubAPI.src.ClubWallet;
using InvestmentClubAPI.src.UserWallet;
using InvestmentClubAPI.src.Transaction;

namespace InvestmentClubAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<InvestmentClub> InvestmentClubs => Set<InvestmentClub>();
    public DbSet<ClubMember> ClubMembers => Set<ClubMember>();
    public DbSet<ClubWallet> ClubWallets => Set<ClubWallet>();
    public DbSet<UserWallet> UserWallets => Set<UserWallet>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
}
