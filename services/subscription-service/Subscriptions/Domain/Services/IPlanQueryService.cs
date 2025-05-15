using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Domain.Model.Queries;

namespace Subscriptions.Domain.Services;

public interface IPlanQueryService
{
    Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query);
}