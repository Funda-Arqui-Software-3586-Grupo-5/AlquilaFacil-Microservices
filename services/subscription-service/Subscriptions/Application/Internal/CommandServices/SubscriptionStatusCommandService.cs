using Subscriptions.Shared.Domain.Repositories;
using Subscriptions.Domain.Model.Commands;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Domain.Model.ValueObjects;
using Subscriptions.Domain.Repositories;
using Subscriptions.Domain.Services;

namespace Subscriptions.Application.Internal.CommandServices;

public class SubscriptionStatusCommandService(ISubscriptionStatusRepository subscriptionStatusRepository, IUnitOfWork unitOfWork) : ISubscriptionStatusCommandService
{
    public async Task Handle(SeedSubscriptionStatusCommand command)
    {
        foreach (ESubscriptionStatus status in Enum.GetValues(typeof(ESubscriptionStatus)))
        {
            if (await subscriptionStatusRepository.ExistsBySubscriptionStatus(status)) continue;
            var subscriptionStatus = new SubscriptionStatus(status.ToString());
            await subscriptionStatusRepository.AddAsync(subscriptionStatus);
        }

        await unitOfWork.CompleteAsync();
    }

}