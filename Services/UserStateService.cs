using System.Text.Json;
using Microsoft.JSInterop;

namespace ScratchPad.Services;

public class UserStateService
{
    private readonly IJSRuntime _jsRuntime;
    private const string UserStateKey = "userState";

    public UserStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveUserStateAsync(UserState userState)
    {
        try
        {
            var json = JsonSerializer.Serialize(userState);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", UserStateKey, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving user state: {ex.Message}");
        }
    }

    public async Task<UserState?> LoadUserStateAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", UserStateKey);
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<UserState>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user state: {ex.Message}");
            return null;
        }
    }


    public async Task ClearUserStateAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", UserStateKey);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing user state: {ex.Message}");
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await ClearUserStateAsync(); // Clears session storage
            Console.WriteLine("User has been logged out.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during logout: {ex.Message}");
        }
    }
}
