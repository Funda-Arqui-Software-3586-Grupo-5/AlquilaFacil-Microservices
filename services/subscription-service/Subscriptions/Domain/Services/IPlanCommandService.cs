using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Commands;
using Subscriptions.Domain.Model.Entities;

namespace Subscriptions.Domain.Services;

public interface IPlanCommandService
{
    public Task<Plan?> Handle(CreatePlanCommand command);

}