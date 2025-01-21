using ScratchPad.Services;
using Microsoft.EntityFrameworkCore;
using ScratchPad.Models;

var builder = WebApplication.CreateBuilder(args);

// Clear default logging providers and add console logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

/* The differences between AddSingleton and AddScoped are:
AddScoped is created per user session
AddSingleton means all users would share the same instance.
This means I shouldnt use add singleton for UIState either*/
builder.Services.AddScoped<UIState>();
builder.Services.AddScoped<UserStateService>();

// Register ApiService with HttpClient using the named client
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://www.mobylmenu.com/api/");
});

// Register DbContext with MySQL connection string
builder.Services.AddDbContext<ScratchPadDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
