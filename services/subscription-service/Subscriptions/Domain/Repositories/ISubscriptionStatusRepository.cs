using Subscriptions.Shared.Domain.Repositories;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Domain.Model.ValueObjects;

namespace Subscriptions.Domain.Repositories;

public interface ISubscriptionStatusRepository : IBaseRepository<SubscriptionStatus>
{
    Task<bool> ExistsBySubscriptionStatus(ESubscriptionStatus subscriptionStatus);
}