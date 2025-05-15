namespace Subscriptions.Interfaces.REST.Resources;

public record SubscriptionResource(int Id, int UserId, int PlanId, int SubscriptionStatusId);