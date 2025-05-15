using Notification.Interfaces.REST.Resources;
using Notification.Domain.Models.Aggregates;

namespace Notification.Interfaces.REST.Transforms;

public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Domain.Models.Aggregates.Notification notification)
    {
        return new NotificationResource
        (
            notification.Id,
            notification.Title,
            notification.Description,
            notification.UserId
        );
    }
}