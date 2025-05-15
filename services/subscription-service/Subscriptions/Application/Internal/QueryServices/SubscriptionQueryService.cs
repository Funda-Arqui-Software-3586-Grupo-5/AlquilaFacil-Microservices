using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Queries;
using Subscriptions.Domain.Repositories;
using Subscriptions.Domain.Services;

namespace Subscriptions.Application.Internal.QueryServices;

public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryServices
{
    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query)
    {
        return await subscriptionRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query)
    {
        return await subscriptionRepository.ListAsync();
    }

    public async Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query)
    {
        return await subscriptionRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Subscription>> Handle(GetSubscriptionsByUserIdQuery query)
    {
        return await subscriptionRepository.FindByUsersIdAsync(query.UserIds);
    }
}