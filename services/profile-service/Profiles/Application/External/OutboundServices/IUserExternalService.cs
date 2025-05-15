namespace Profiles.Application.External.OutboundServices;

public interface IUserExternalService
{
    Task<bool> UserExistsById(int userId);
}