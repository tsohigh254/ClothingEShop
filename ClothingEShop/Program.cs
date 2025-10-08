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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    "Host=localhost;Database=clothingeshop;Username=postgres;Password=password;";

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
app.MapGet("/", () => "Clothing E-Shop API is running!");

// Map API controllers
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Create database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClothingEShopDbContext>();
    try
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Initializing database...");
        
        context.Database.EnsureCreated();
        logger.LogInformation("Database initialized successfully.");
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database initialization failed: {Message}", ex.Message);
    }
}

app.Run();
