
using Subscriptions.Application.External.OutBoundServices;

namespace Subscriptions.Infrastructure.IAM;

public class ExternalUserWithSubscriptionService(HttpClient client) : IExternalUserWithSubscriptionService
{
    public async Task<bool> UserExists(int id)
    {
        var url = "http://iam-api:8012/api/v1/users/" + id + "/exists";
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error while checking user existence");
        }

        var content = await response.Content.ReadAsStringAsync();
        return bool.Parse(content);
    }
} 