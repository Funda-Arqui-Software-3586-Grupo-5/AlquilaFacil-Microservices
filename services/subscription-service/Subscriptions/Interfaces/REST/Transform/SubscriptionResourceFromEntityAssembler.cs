using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Interfaces.REST.Resources;
using Microsoft.OpenApi.Extensions;

namespace Subscriptions.Interfaces.REST.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity)
    {
        return new SubscriptionResource(entity.Id, entity.UserId, entity.PlanId,
            entity.SubscriptionStatusId);
    }
}