using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

public class ApiService
{
    // Define the HttpClient, making it read only to make sure it's not changed
    private readonly HttpClient _httpClient;

    // Define a private field to cache the user activity, so we dont have to fetch it every time
    private UserActivitySummary _cachedUserActivity;

    // Constructor, setting the HttpClient from the Program.cs
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // My method to fetch user analytics from MM API
    public async Task<UserActivitySummary> GetUserAnalyticsAsync()
    {
        if (_cachedUserActivity != null)
        {
            Console.WriteLine("Returning cached user analytics.");
            return _cachedUserActivity;
        }

        try
        {
            // Create a new HttpRequest with my hardcoded token
            var request = new HttpRequestMessage(HttpMethod.Get, "user_analytics/");
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Token", "912370917e0b8f88dbad53f4eca993ab650f5832");

            // Send the request
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Deserialize the response
            _cachedUserActivity = await response.Content.ReadFromJsonAsync<UserActivitySummary>();

            return _cachedUserActivity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user analytics: {ex.Message}");
            throw;
        }
    }

    public void ClearUserAnalyticsCache()
    {
        Console.WriteLine("Clearing user analytics cache.");
        _cachedUserActivity = null;
    }
}

// Model for user activity, use JsonPropertyName to account for snake case coming from API
public class UserActivitySummary
{
    [JsonPropertyName("total_users")]
    public int TotalUsers { get; set; }

    [JsonPropertyName("daily_active_users")]
    public int DailyActiveUsers { get; set; }

    [JsonPropertyName("weekly_active_users")]
    public int WeeklyActiveUsers { get; set; }

    [JsonPropertyName("monthly_active_users")]
    public int MonthlyActiveUsers { get; set; }
}
