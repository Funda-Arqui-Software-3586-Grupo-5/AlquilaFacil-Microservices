using System.Text.Json;

namespace Booking.Interfaces.ACL.Services;

public class IamContextFacade(HttpClient httpClient) : IIamContextFacade
{
    public async Task<bool> UserExists(int userId)
    {
        var endpoint = $"http://iam-api:8012/api/v1/users/{userId}/exists";
        var response = await httpClient.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return bool.Parse(content);
        }

        // Handle error response
        throw new Exception($"Error checking user existence: {response.StatusCode}");
    }
}