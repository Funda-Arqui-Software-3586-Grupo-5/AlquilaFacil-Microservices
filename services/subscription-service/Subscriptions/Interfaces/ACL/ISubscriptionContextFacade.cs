using Subscriptions.Domain.Model.Aggregates;

namespace Subscriptions.Interfaces.ACL;

public interface ISubscriptionContextFacade
{
    Task<IEnumerable<Subscription>> GetSubscriptionByUsersId(List<int> usersId);
    Task<bool> IsUserSubscribed(int userId);
}