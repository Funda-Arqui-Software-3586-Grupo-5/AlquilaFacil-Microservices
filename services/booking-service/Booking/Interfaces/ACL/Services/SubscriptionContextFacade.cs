using Booking.Interfaces.ACL.DTOs;
using System.Text.Json;

namespace Booking.Interfaces.ACL.Services;

public class SubscriptionContextFacade(HttpClient httpClient) : ISubscriptionContextFacade
{
    public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionsByUsersId(List<int> usersId)
    {
        var endpoint = $"http://subscription-api:8016/api/v1/subscriptions/subscriptions/by-users";
        // put the query param as array with usersId=
        var query = string.Join("&", usersId.Select(id => $"usersId={id}"));
        endpoint += $"?{query}";
        var response = await httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching subscriptions: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<SubscriptionDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}