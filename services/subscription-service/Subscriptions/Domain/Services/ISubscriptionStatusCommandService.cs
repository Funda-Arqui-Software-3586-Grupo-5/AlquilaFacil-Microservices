using Subscriptions.Domain.Model.Commands;

namespace Subscriptions.Domain.Services;

public interface ISubscriptionStatusCommandService
{
    Task Handle(SeedSubscriptionStatusCommand command);
}