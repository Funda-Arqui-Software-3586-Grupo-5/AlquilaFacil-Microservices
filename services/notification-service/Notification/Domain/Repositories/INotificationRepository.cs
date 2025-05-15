using Notification.Shared.Domain.Repositories;
using Notification.Domain.Models.Aggregates;

namespace Notification.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<Models.Aggregates.Notification>
{
    Task<IEnumerable<Models.Aggregates.Notification>> GetNotificationsByUserId(int userId);
}