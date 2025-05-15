namespace Notification.Interfaces.REST.Resources;

public record CreateNotificationResource(string Title, string Description, int UserId);