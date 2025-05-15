using Booking.Interfaces.ACL.DTOs;

namespace Booking.Interfaces.ACL;

public interface ILocalsContextFacade
{
    Task<bool> LocalExists(int reservationId);
    Task<IEnumerable<LocalDto>> GetLocalsByUserId(int userId);
    Task<bool> IsLocalOwner(int userId, int localId);
}
