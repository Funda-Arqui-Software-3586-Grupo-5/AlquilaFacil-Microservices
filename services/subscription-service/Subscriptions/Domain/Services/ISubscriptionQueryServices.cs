using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Queries;

namespace Subscriptions.Domain.Services;

public interface ISubscriptionQueryServices
{
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);
    Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query);
    
    Task<Subscription?> Handle(GetSubscriptionByUserIdQuery query);
    Task<IEnumerable<Subscription>> Handle(GetSubscriptionsByUserIdQuery query);
}