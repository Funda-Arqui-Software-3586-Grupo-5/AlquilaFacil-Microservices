using System.Text.Json;
using Booking.Interfaces.ACL.DTOs;

namespace Booking.Interfaces.ACL.Services;

public class LocalsContextFacade : ILocalsContextFacade
{
    private readonly HttpClient _httpClient;

    public LocalsContextFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LocalExists(int reservationId)
    {
        var endpoint = $"http://local-api:8013/api/v1/locals/{reservationId}";
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error checking local existence: {response.StatusCode}");
        }
       
        var content = await response.Content.ReadAsStringAsync();
        if(string.IsNullOrEmpty(content))
        {
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<LocalDto>> GetLocalsByUserId(int userId)
    {
        var endpoint = $"http://local-api:8013/api/v1/locals/get-user-locals/{userId}";
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching locals: {response.StatusCode}");
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<LocalDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<bool> IsLocalOwner(int userId, int localId)
    {
        var endpoint = $"http://local-api:8013/api/v1/locals/get-user-locals/{userId}";
        var response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error checking local ownership: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var locals = JsonSerializer.Deserialize<IEnumerable<LocalDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        if (locals == null || !locals.Any())
        {
            return false;
        }
        return true;
    }
}
