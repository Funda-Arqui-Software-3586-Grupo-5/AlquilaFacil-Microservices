namespace Booking.Interfaces.REST.Resources;

public record LocalReservationResource(int Id,DateTime StartDate, DateTime EndDate, int UserId, int LocalId, bool IsSubscribe);