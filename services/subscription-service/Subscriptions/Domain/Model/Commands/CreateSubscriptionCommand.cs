namespace Subscriptions.Domain.Model.Commands;

public record CreateSubscriptionCommand(int UserId,int PlanId);