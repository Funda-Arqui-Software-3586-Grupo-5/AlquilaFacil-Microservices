using Subscriptions.Shared.Infrastructure.Persistence.EFC.Configuration;
using Subscriptions.Shared.Infrastructure.Persistence.EFC.Repositories;
using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Domain.Repositories;

namespace Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class PlanRepository(AppDbContext context) : BaseRepository<Plan>(context), IPlanRepository;