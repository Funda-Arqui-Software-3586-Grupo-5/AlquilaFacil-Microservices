namespace LocalManagement.Application.External.OutboundServices;

public interface IUserExternalService
{
    public Task<bool> UserExists(int userId);
}