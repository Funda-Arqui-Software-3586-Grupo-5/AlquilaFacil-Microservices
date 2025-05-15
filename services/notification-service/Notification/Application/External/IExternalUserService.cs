namespace Notification.Application.External;

public interface IExternalUserService
{
    Task<bool> IsUserExistsAsync(int profileId);
}