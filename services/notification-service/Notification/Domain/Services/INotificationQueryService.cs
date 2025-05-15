using Notification.Domain.Models.Queries;
using Notification.Domain.Models.Aggregates;

namespace Notification.Domain.Services;

public interface INotificationQueryService
{
    Task<IEnumerable<Models.Aggregates.Notification>> Handle(GetNotificationsByUserIdQuery query);
}