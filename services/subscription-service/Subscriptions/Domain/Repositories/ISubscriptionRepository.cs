using Subscriptions.Shared.Domain.Repositories;
using Subscriptions.Domain.Model.Aggregates;

namespace Subscriptions.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindByUserIdAsync(int userId);
    Task<IEnumerable<Subscription>> FindByUsersIdAsync(List<int> usersId);
}