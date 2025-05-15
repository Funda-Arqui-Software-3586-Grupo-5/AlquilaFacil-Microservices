using Booking.Interfaces.ACL.DTOs;

namespace Booking.Interfaces.ACL;

public interface ISubscriptionContextFacade
{
    Task<IEnumerable<SubscriptionDto>> GetSubscriptionsByUsersId(List<int> usersId);
}
