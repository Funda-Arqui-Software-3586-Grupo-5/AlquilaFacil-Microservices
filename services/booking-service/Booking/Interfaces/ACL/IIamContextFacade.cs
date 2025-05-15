namespace Booking.Interfaces;

public interface IIamContextFacade
{
    Task<bool> UserExists(int userId);
}