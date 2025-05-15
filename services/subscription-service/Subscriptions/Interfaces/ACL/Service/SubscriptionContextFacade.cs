using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Queries;
using Subscriptions.Domain.Model.ValueObjects;
using Subscriptions.Domain.Services;

namespace Subscriptions.Interfaces.ACL.Service;

public class SubscriptionContextFacade(ISubscriptionQueryServices subscriptionQueryServices) : ISubscriptionContextFacade
{
    

    public async Task<IEnumerable<Subscription>> GetSubscriptionByUsersId(List<int> usersId)
    {
        var query = new GetSubscriptionsByUserIdQuery(usersId);
        var subscriptions = await subscriptionQueryServices.Handle(query);  
        return subscriptions;
    }

    public async Task<bool> IsUserSubscribed(int userId)
    {
        var query = new GetSubscriptionByUserIdQuery(userId);
        var subscription = await subscriptionQueryServices.Handle(query);
        return subscription != null;

    }
}