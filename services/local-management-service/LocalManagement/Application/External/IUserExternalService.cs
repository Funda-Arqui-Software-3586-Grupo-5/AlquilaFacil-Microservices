namespace LocalManagement.Application.External;

public interface IUserExternalService
{
    public Task<bool> UserExists(int userId);
}