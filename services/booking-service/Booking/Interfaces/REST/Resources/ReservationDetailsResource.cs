//using AlquilaFacilPlatform.Subscriptions.Interfaces.REST.Resources;

namespace Booking.Interfaces.REST.Resources;

public record ReservationDetailsResource(IEnumerable<LocalReservationResource> Reservations);