using InvestmentClubAPI.Data;
using InvestmentClubAPI.src.AuditLog;
using Microsoft.EntityFrameworkCore;

namespace InvestmentClubAPI.src.UserAuth;

public class AuthEndpointsContext { }

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", async (
            RegisterRequest req,
            AppDbContext db,
            AuditService audit,
            ILogger<AuthEndpointsContext> logger
        ) =>
        {
            logger.LogInformation("Register attempt for username {Username}", req.Username);

            // Check if user exists
            if (await db.Users.AnyAsync(u => u.Username == req.Username))
            {
                logger.LogWarning("Register failed: username {Username} already exists", req.Username);
                return Results.BadRequest("Username already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);

            var user = new User
            {
                Username = req.Username,
                Password = hashedPassword
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            await audit.LogAsync(user.Id, "CREATE_USER", "User", user.Id, $"Username: {user.Username}");

            logger.LogInformation("User {Username} registered successfully with Id {UserId}", req.Username, user.Id);

            return Results.Ok(new { user.Id, user.Username });
        });

        group.MapPost("/login", async (
            LoginRequest req,
            AuthService service,
            AuditService audit,
            AppDbContext db,
            ILogger<AuthEndpointsContext> logger
        ) =>
        {
            try
            {
                var authResponse = await service.LoginAsync(req.Username, req.Password);

                var user = await db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
                if (user != null)
                {
                    await audit.LogAsync(user.Id, "LOGIN_SUCCESS", "User", user.Id, $"Username: {req.Username}");
                }

                logger.LogInformation("User {Username} logged in successfully", req.Username);
                return Results.Ok(authResponse);
            }
            catch
            {
                logger.LogWarning("Login failed for username {Username}", req.Username);
                return Results.Unauthorized();
            }
        });
    }

    public static IServiceCollection AddAuthModule(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<AuditService>();
        return services;
    }
}
