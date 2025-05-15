using Subscriptions.Shared.Infrastructure.Persistence.EFC.Configuration;
using Subscriptions.Shared.Infrastructure.Persistence.EFC.Repositories;
using Subscriptions.Domain.Model.Entities;
using Subscriptions.Domain.Model.ValueObjects;
using Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionStatusRepository(AppDbContext context) : BaseRepository<SubscriptionStatus>(context), ISubscriptionStatusRepository
{
    public async Task<bool> ExistsBySubscriptionStatus(ESubscriptionStatus subscriptionStatus)
    {
        return await Context.Set<SubscriptionStatus>().AnyAsync(x => x.Status == subscriptionStatus.ToString());
    }
}