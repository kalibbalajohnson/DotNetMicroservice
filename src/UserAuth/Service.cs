using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using InvestmentClubAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InvestmentClubAPI.src.UserAuth;

public class AuthService
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _db;

    public AuthService(IConfiguration config, AppDbContext db)
    {
        _config = config;
        _db = db;
    }

    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        var user = await _db.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            throw new UnauthorizedAccessException();

        var accessToken = GenerateJwt(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        return new AuthResponse(accessToken, refreshToken.Token);
    }

    public async Task<AuthResponse> RefreshAsync(string refreshToken)
    {
        var token = await _db.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                t.Token == refreshToken &&
                !t.IsRevoked &&
                t.ExpiresAt > DateTime.UtcNow);

        if (token == null)
            throw new UnauthorizedAccessException();

        token.IsRevoked = true;

        var newRefresh = GenerateRefreshToken();
        token.User.RefreshTokens.Add(newRefresh);

        await _db.SaveChangesAsync();

        return new AuthResponse(
            GenerateJwt(token.User),
            newRefresh.Token
        );
    }

    private string GenerateJwt(User user)
    {
        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            jwt["Issuer"],
            jwt["Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(jwt["ExpiresInMinutes"]!)
            ),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }
}