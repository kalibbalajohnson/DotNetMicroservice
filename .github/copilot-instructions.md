## Purpose

Short, concrete instructions for AI code helpers working on this repository. Focus on the project's minimal-API structure, EF Core/Postgres integration, and developer workflows.

## Big picture (what this project is)

- Minimal ASP.NET Core Web API (top-level statements) targeting .NET 9.0.
- Data access via EF Core (Npgsql provider) in `Data/AppDbContext.cs` with a single DbSet: `Products`.
- Minimal endpoints are defined directly in `Program.cs` (no Controllers). Example routes: `GET /products`, `POST /products`.
- OpenAPI generation is enabled via `Microsoft.AspNetCore.OpenApi` (JSON only). There is no Swagger UI configured.

## Key files to consult

- `Program.cs` — central place for routing, DI, and OpenAPI wiring.
- `Data/AppDbContext.cs` — EF Core DbContext, contains `DbSet<Product> Products`.
- `Models/Product.cs` — product model (Id, Name, Price). Note: nullable reference types are enabled.
- `appsettings.json` / `appsettings.Development.json` — connection strings and environment settings.
- `MyMicroservice.csproj` — target framework and package references (Npgsql, EF Tools, OpenApi).
- `MyMicroservice.http` — example HTTP requests; useful for manual testing.
- `Properties/launchSettings.json` — applicationUrl(s) used when running locally (e.g. http://localhost:5139).

## Project-specific patterns and conventions

- Minimal API pattern: add endpoints using `app.MapGet(...)`, `app.MapPost(...)` in `Program.cs`.
- DbContext is registered with DI in `Program.cs` using `builder.Services.AddDbContext<AppDbContext>(...)` and resolved in route handlers by parameter injection (e.g., `async (AppDbContext db) => ...`).
- Use configuration key `ConnectionStrings:DefaultConnection` (defined in `appsettings.json`) for the Npgsql connection string.
- Nullable reference types are enabled in the project (`<Nullable>enable</Nullable>`). When creating or modifying model properties, be explicit about nullability.

## Build / run / debug (concrete commands)

- Build: `dotnet build` from repository root.
- Run: `dotnet run` (launches URLs in `Properties/launchSettings.json`; default HTTP URL is `http://localhost:5139`).
- OpenAPI JSON: after running, GET the OpenAPI document at `/openapi` (MapOpenApi is used). There is no swagger UI by default.

## Database and migrations (what's present / how to act)

- The project uses EF Core with the Npgsql provider (see `MyMicroservice.csproj`).
- Connection string: `appsettings.json` -> `ConnectionStrings:DefaultConnection` (points to Postgres on localhost by default).
- Tools: `Microsoft.EntityFrameworkCore.Tools` is referenced. Typical, discoverable commands:
  - Add a migration: `dotnet ef migrations add <Name>` (requires the `dotnet-ef` CLI—install with `dotnet tool install --global dotnet-ef` if missing).
  - Apply migrations: `dotnet ef database update`.

## Examples (copyable snippets referencing real code)

- GET products (returns all rows):
  GET http://localhost:5139/products

- POST create product (JSON body matches `Models/Product`):
  POST http://localhost:5139/products
  Content-Type: application/json
  Body: { "name": "Widget", "price": 9.99 }

## Small rules for AI edits (do these specifically for this repo)

1. Keep changes in `Program.cs` minimal and consistent with the existing minimal-API style — prefer `app.Map...` handlers over creating Controllers.
2. When introducing new services, register them in the DI container in `Program.cs` near the `AddDbContext` call.
3. If modifying the model shape (e.g., `Product`), add an EF migration and update the database; mention this in the PR description.
4. Use `builder.Configuration.GetConnectionString("DefaultConnection")` for DB wiring — don't hardcode connection strings.
5. Because OpenAPI is JSON-only here, do not add Swagger UI unless the PR also updates program startup to opt-in the UI and includes a short justification.

## Where to look when something fails

- If DB errors occur: check `appsettings.json` connection string and ensure Postgres is reachable on localhost:5432.
- If DI resolution fails: ensure types are registered before `app.Build()` in `Program.cs`.
- If routes are missing: confirm `Program.cs` contains `app.MapGet`/`app.MapPost` for the route and that `app.Run()` is executed.

## Quick PR checklist for changes an AI should include in its PR

- Which files changed and why (brief).
- If the DB model changed: include migration files and note how to run `dotnet ef database update`.
- Note any new package references and why.

---

If you'd like, I can (1) add a brief sample test harness, (2) wire up Swagger UI behind an env flag, or (3) add a CI check that runs `dotnet build` and `dotnet ef migrations list` — tell me which and I'll proceed.
