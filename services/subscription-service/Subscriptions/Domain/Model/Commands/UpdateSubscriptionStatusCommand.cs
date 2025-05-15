namespace Subscriptions.Domain.Model.Commands;

public record UpdateSubscriptionStatusCommand(int Id, int StatusId);