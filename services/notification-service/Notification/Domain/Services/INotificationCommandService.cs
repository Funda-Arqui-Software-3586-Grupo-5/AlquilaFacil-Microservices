using Notification.Domain.Models.Commands;
using Notification.Domain.Models.Aggregates;

namespace Notification.Domain.Services;

public interface INotificationCommandService
{
    Task<Models.Aggregates.Notification> Handle(CreateNotificationCommand command);
    Task<Models.Aggregates.Notification> Handle(DeleteNotificationCommand command);
}