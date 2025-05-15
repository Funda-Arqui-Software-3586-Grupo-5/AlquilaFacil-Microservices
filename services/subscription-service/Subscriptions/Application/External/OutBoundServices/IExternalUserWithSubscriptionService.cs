namespace Subscriptions.Application.External.OutBoundServices;

public interface IExternalUserWithSubscriptionService
{
    Task<bool> UserExists(int id);
}