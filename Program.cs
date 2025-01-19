using ScratchPad.Services;

var builder = WebApplication.CreateBuilder(args);

// Clear default logging providers and add console logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<UIState>();

// Register ApiService with HttpClient using the named client
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://www.mobylmenu.com/api/");
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
