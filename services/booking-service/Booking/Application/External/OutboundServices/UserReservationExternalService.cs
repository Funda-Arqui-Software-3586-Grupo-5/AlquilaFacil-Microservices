//using AlquilaFacilPlatform.IAM.Interfaces.ACL;

using Booking.Interfaces;

namespace Booking.Application.External.OutboundServices;
public class UserReservationExternalService(IIamContextFacade iamContextFacade)
    : IUserReservationExternalService
{
    public async Task<bool> UserExists(int userId)
    {
        return await iamContextFacade.UserExists(userId);
    }
}