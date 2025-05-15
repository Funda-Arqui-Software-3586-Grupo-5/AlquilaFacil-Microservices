namespace Booking.Application.External;

public interface IUserReservationExternalService
{
    Task<bool> UserExists(int userId);
}