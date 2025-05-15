using LocalManagement.Interfaces.ACL;

namespace LocalManagement.Application.External.OutboundServices;

public class UserExternalService(IIamContextFacade iamContextFacade) : IUserExternalService
{
    public async Task<bool> UserExists(int userId)
    {
        return await iamContextFacade.UserExists(userId);
    }
}