using ClothingEShop.Components;
using ClothingEShop.Data;
using ClothingEShop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add API controllers
builder.Services.AddControllers();

// Add CORS for API access
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Entity Framework
string connectionString = BuildConnectionString(builder.Configuration);

// Log the connection attempt (without sensitive data)
var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger("DatabaseConfig");
logger.LogInformation("Attempting database connection to: {Host}", 
    connectionString.Split(';').FirstOrDefault(x => x.StartsWith("Host=")) ?? "Unknown");

builder.Services.AddDbContext<ClothingEShopDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services
builder.Services.AddScoped<IProductService, ProductService>();

// Add HttpClient for API calls from Blazor components
builder.Services.AddHttpClient();

// Add health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Remove HTTPS redirection for containerized environments
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

// Use CORS
app.UseCors();

// Health check endpoints
app.MapHealthChecks("/health");
app.MapGet("/api/status", () => "Clothing E-Shop API is running!");

// Map API controllers
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Create database and apply migrations
await InitializeDatabaseAsync(app.Services);

app.Run();

// Helper method to build connection string
static string BuildConnectionString(IConfiguration configuration)
{
    // Try to get connection string from various sources
    var configConnectionString = configuration.GetConnectionString("DefaultConnection");
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    
    // Check for individual environment variables (common in container environments)
    var host = Environment.GetEnvironmentVariable("DB_HOST") ?? Environment.GetEnvironmentVariable("POSTGRES_HOST");
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
    var database = Environment.GetEnvironmentVariable("DB_NAME") ?? Environment.GetEnvironmentVariable("POSTGRES_DB");
    var username = Environment.GetEnvironmentVariable("DB_USER") ?? Environment.GetEnvironmentVariable("POSTGRES_USER");
    var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

    if (!string.IsNullOrEmpty(databaseUrl))
    {
        // Parse DATABASE_URL format (commonly used by Render, Heroku, etc.)
        // postgres://username:password@host:port/database
        try
        {
            var uri = new Uri(databaseUrl);
            var userInfo = uri.UserInfo.Split(':');
            return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.Trim('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
        }
        catch (Exception)
        {
            throw new InvalidOperationException($"Invalid DATABASE_URL format: {databaseUrl}");
        }
    }
    else if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(database) && 
             !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
    {
        // Build from individual environment variables
        return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Prefer;Trust Server Certificate=true;";
    }
    else if (!string.IsNullOrEmpty(configConnectionString))
    {
        return configConnectionString;
    }
    else
    {
        // Fallback connection string for development
        return "Host=localhost;Database=clothingeshop;Username=postgres;Password=password;Port=5432;";
    }
}

// Helper method to initialize database
static async Task InitializeDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ClothingEShopDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Initializing database...");
        
        // Test the connection first
        await context.Database.CanConnectAsync();
        
        // Create database if it doesn't exist
        await context.Database.EnsureCreatedAsync();
        
        logger.LogInformation("Database initialized successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database initialization failed: {Message}", ex.Message);
        logger.LogWarning("Application will continue without database connectivity");
        
        // Don't fail the application startup if database is not available
        // This allows the app to start even if the database is temporarily unavailable
    }
}
