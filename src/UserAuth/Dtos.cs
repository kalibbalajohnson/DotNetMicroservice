namespace InvestmentClubAPI.src.UserAuth;

public record RegisterRequest(string Username, string Password);
public record LoginRequest(string Username, string Password);
public record RefreshRequest(string RefreshToken);
public record AuthResponse(string AccessToken, string RefreshToken);