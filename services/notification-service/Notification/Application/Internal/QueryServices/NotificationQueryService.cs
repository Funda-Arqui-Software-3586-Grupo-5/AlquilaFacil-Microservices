using Notification.Domain.Models.Queries;
using Notification.Domain.Repositories;
using Notification.Domain.Services;
using Notification.Domain.Models.Aggregates;

namespace Notification.Application.Internal.QueryServices;

public class NotificationQueryService(INotificationRepository notificationRepository) : INotificationQueryService
{
    public async Task<IEnumerable<Domain.Models.Aggregates.Notification>> Handle(GetNotificationsByUserIdQuery query)
    {
        return await notificationRepository.GetNotificationsByUserId(query.UserId);
    }
}