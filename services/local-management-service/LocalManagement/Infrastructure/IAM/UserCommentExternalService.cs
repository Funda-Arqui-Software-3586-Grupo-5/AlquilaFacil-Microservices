using LocalManagement.Application.External.OutboundServices;

namespace LocalManagement.Infrastructure.IAM;

public class UserCommentExternalService(HttpClient httpClient) : IUserCommentExternalService
{

    public async Task<bool> UserExists(int userId)
    {
        var endpoint = $"http://iam-api:8012/api/v1/users/{userId}/exists";
        var response = await httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error calling external service: {response.StatusCode}");
        }
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);
        return bool.Parse(result);
        

    }
}