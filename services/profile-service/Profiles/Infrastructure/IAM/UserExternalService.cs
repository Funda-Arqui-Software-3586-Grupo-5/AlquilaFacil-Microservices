using Profiles.Application.External.OutboundServices;

namespace Profiles.Infrastructure.IAM;

public class UserExternalService(HttpClient client) : IUserExternalService
{
    public async Task<bool> UserExistsById(int userId)
    {
        var url = "http://iam-api:8012/api/v1/users/" + userId + "/exists";
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error while checking user existence");
        }

        var content = await response.Content.ReadAsStringAsync();
        return bool.Parse(content);
    }
}