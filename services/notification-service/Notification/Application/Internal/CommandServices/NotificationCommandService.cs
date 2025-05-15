using Notification.Application.External;
using Notification.Domain.Models.Commands;
using Notification.Domain.Repositories;
using Notification.Domain.Services;
using Notification.Shared.Domain.Repositories;

namespace Notification.Application.Internal.CommandServices;

public class NotificationCommandService(
    IExternalUserService externalUserService,
    IUnitOfWork unitOfWork,
    INotificationRepository notificationRepository
    ) : INotificationCommandService
{
    public async Task<Domain.Models.Aggregates.Notification> Handle(CreateNotificationCommand command)
    {
        var notification = new Domain.Models.Aggregates.Notification(command);
        var isUserExists = await externalUserService.IsUserExistsAsync(command.UserId);
        if (!isUserExists)
        {
            throw new Exception("This user does not exist");
        }
        await notificationRepository.AddAsync(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }

    public async Task<Domain.Models.Aggregates.Notification> Handle(DeleteNotificationCommand command)
    {
        var notification = await notificationRepository.FindByIdAsync(command.Id);
        if (notification == null)
        {
            throw new Exception("Notification not found");
        }
        notificationRepository.Remove(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }
}