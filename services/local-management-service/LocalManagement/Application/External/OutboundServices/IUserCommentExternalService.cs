namespace LocalManagement.Application.External.OutboundServices;

public interface IUserCommentExternalService
{
    Task<bool> UserExists(int userId);
}