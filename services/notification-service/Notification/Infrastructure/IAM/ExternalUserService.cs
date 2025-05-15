using Notification.Application.External;

namespace Notification.Infrastructure.IAM;

public class ExternalUserService(HttpClient client) : IExternalUserService
{
    public async Task<bool> IsUserExistsAsync(int profileId)
    {
       var url = "http://iam-api:8012/api/v1/users/" + profileId + "/exists";
       var response = await client.GetAsync(url);
       if (!response.IsSuccessStatusCode)
       {
              throw new Exception("IAM microservice doesnt working");
       }
       var result = await response.Content.ReadAsStringAsync();
       return bool.Parse(result);
       
    }
}