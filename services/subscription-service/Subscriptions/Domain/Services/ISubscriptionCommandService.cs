using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Commands;

namespace Subscriptions.Domain.Services;

public interface ISubscriptionCommandService
{
    public Task<Subscription?> Handle(CreateSubscriptionCommand command);

    public Task<Subscription?> Handle(UpdateSubscriptionStatusCommand command);
}