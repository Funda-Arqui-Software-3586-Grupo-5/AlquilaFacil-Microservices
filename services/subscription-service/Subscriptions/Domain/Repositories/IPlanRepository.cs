using Subscriptions.Shared.Domain.Repositories;
using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Entities;

namespace Subscriptions.Domain.Repositories;

public interface IPlanRepository : IBaseRepository<Plan>
{
}