namespace Profiles.Application.External.OutboundServices;

public interface ISubscriptionExternalService
{
    Task<bool> IsUserSubscribeAsync(int userId);
}