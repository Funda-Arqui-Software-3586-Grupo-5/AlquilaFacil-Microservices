//using Subscriptions.IAM.Domain.Model.Aggregates;

using Subscriptions.Application.External;
using Subscriptions.Application.External.OutBoundServices;
using Subscriptions.Shared.Domain.Repositories;
using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Commands;
using Subscriptions.Domain.Repositories;
using Subscriptions.Domain.Services;

namespace Subscriptions.Application.Internal.CommandServices;

public class SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, ISubscriptionStatusRepository subscriptionStatusRepository,
    IPlanRepository planRepository, 
    IUnitOfWork unitOfWork, IExternalUserWithSubscriptionService externalUserWithSubscriptionService)
    : ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateSubscriptionCommand command)
    {
        var subscription = new Subscription(command);

        var plan = await planRepository.FindByIdAsync(command.PlanId);
        if (plan == null)
        {
            throw new Exception("Plan not found");
        }
        var isUserExists = await externalUserWithSubscriptionService.UserExists(command.UserId);
        if (!isUserExists)
        {
            throw new Exception("User not found");
        }
        
        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }

    public async Task<Subscription?> Handle(UpdateSubscriptionStatusCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.Id);
        var subscriptionStatus = await subscriptionStatusRepository.FindByIdAsync(command.StatusId);
        if ( subscription == null || subscriptionStatus == null)
        {
            throw new Exception("Subscription or Status not found");
        }
        subscription.SubscriptionStatusId = subscriptionStatus.Id;
        await unitOfWork.CompleteAsync();
        return subscription;
    }
}