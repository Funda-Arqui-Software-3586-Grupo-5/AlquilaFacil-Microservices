using Booking.Domain.Model.Aggregates;
using Booking.Domain.Model.Commands;

namespace Booking.Domain.Services;

public interface IReservationCommandService
{
    Task<Reservation> Handle(CreateReservationCommand reservation);
    Task<Reservation> Handle(UpdateReservationDateCommand reservation);
    Task<Reservation> Handle(DeleteReservationCommand reservation);
}