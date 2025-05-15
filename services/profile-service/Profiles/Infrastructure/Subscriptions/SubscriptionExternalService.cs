using Profiles.Application.External.OutboundServices;

namespace Profiles.Infrastructure.Subscriptions;

public class SubscriptionExternalService(HttpClient client) : ISubscriptionExternalService
{
    public async Task<bool> IsUserSubscribeAsync(int userId)
    {
        var url = "http://subscription-api:8016/api/v1/subscriptions/" + userId + "/subscribed";
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error while checking user existence");
        }

        var content = await response.Content.ReadAsStringAsync();
        return bool.Parse(content);
    }
}