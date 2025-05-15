using Microsoft.EntityFrameworkCore;
using Notification.Domain.Repositories;
using Notification.Shared.Infrastructure.Persistence.EFC.Configuration;
using Notification.Shared.Infrastructure.Persistence.EFC.Repositories;
using Notification.Domain.Models.Aggregates;
using Notifications.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace Notification.Infrastructure.Persistence.EFC.Repositories;

public class NotificationRepository(AppDbContext context) : BaseRepository<Domain.Models.Aggregates.Notification>(context), INotificationRepository
{
    public async Task<IEnumerable<Domain.Models.Aggregates.Notification>> GetNotificationsByUserId(int userId)
    {
        return await Context.Set<Domain.Models.Aggregates.Notification>().Where(n => n.UserId == userId).ToListAsync();
    }
}