using Subscriptions.Shared.Domain.Repositories;
using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Commands;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Domain.Repositories;
using Subscriptions.Domain.Services;

namespace Subscriptions.Application.Internal.CommandServices;

public class PlanCommandService(IPlanRepository planRepository, IUnitOfWork unitOfWork) : IPlanCommandService
{
    public async Task<Plan?> Handle(CreatePlanCommand command)
    {
        if (command.Price <= 0)
        {
            throw new Exception("Plan price cannot be negative or less than 0");
        }
        var plan = new Plan(command.Name, command.Service, command.Price);
        await planRepository.AddAsync(plan);
        await unitOfWork.CompleteAsync();
        return plan;
    }
}