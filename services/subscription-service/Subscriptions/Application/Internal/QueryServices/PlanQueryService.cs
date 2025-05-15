using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Domain.Model.Queries;
using Subscriptions.Domain.Repositories;
using Subscriptions.Domain.Services;

namespace Subscriptions.Application.Internal.QueryServices;

public class PlanQueryService(IPlanRepository planRepository) : IPlanQueryService
{
    public async Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query)
    {
        return await planRepository.ListAsync();
    }
}