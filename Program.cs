using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ScratchPad.Services;
using ScratchPad.Models;
using http.context;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddApplicationInsights(); // Enables telemetry for both API and Functions

// Configuration (Loads appsettings.json & environment variables)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()) // Ensure it reads from root
    .AddJsonFile("host.json", optional: true, reloadOnChange: true)
    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register Blazor Services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<UIState>();
builder.Services.AddScoped<UserStateService>();

// Register HttpClient for API Calls
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://www.mobylmenu.com/api/");
});

// ✅ Register DbContext **ONLY ONCE** (works for both API & Functions)
builder.Services.AddDbContext<ScratchPadDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// ✅ Register Azure Functions Worker (Allows Functions to Use the Same DI)
builder.Host.ConfigureFunctionsWorkerDefaults();

// ✅ Register Azure Functions Telemetry
builder.Services.AddApplicationInsightsTelemetryWorkerService();
builder.Services.ConfigureFunctionsApplicationInsights();

var app = builder.Build();

// Middleware & Routing
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Start the application
app.Run();