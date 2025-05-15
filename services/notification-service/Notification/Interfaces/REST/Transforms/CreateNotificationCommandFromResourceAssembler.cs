using Notification.Domain.Models.Commands;
using Notification.Interfaces.REST.Resources;

namespace Notification.Interfaces.REST.Transforms;

public static class CreateNotificationCommandFromResourceAssembler
{
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource createNotificationResource)
    {
        return new CreateNotificationCommand(
            createNotificationResource.Title,
            createNotificationResource.Description,
            createNotificationResource.UserId
        );
    }
}