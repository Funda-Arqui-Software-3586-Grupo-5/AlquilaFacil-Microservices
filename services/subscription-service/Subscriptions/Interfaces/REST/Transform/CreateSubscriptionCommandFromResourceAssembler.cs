using Subscriptions.Domain.Model.Commands;
using Subscriptions.Interfaces.REST.Resources;

namespace Subscriptions.Interfaces.REST.Transform;

public static class CreateSubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommandFromResource(CreateSubscriptionResource resource)
    {
        return new CreateSubscriptionCommand(resource.UserId,resource.PlanId);
    }
}