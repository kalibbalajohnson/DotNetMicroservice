using Microsoft.EntityFrameworkCore;
using MyMicroservice.Data;
using MyMicroservice.Models;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Important: Swagger middleware comes BEFORE mapping endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();               // Serves JSON at /swagger/v1/swagger.json
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyMicroservice API V1");
        c.RoutePrefix = "swagger"; // Default is 'swagger', can also be "" for root
    });
}

// Optional root endpoint
app.MapGet("/", () => "MyMicroservice API is running");

// Products endpoints
app.MapGet("/products", async (AppDbContext db) =>
{
    return await db.Products.ToListAsync();
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.Run();