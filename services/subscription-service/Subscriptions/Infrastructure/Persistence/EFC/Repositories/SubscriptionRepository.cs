using Subscriptions.Shared.Infrastructure.Persistence.EFC.Configuration;
using Subscriptions.Shared.Infrastructure.Persistence.EFC.Repositories;
using Subscriptions.Domain.Model.Aggregates;
using Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Subscriptions.Infrastructure.Persistence.EFC.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<Subscription?> FindByUserIdAsync(int userId)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }
    
    public async Task<IEnumerable<Subscription>> FindByUsersIdAsync(List<int> usersId)
    {
        return await Context.Set<Subscription>()
            .Where(s => usersId.Contains(s.UserId))
            .ToListAsync();
    }
}