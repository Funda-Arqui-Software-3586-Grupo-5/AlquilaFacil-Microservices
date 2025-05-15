using Booking.Domain.Model.Aggregates;
using Booking.Domain.Model.Queries;
//using Subscriptions.Domain.Model.Aggregates;

namespace Booking.Domain.Services;

public interface IReservationQueryService
{
   
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(GetReservationsByUserId query);
    Task<IEnumerable<Reservation>>GetReservationByStartDateAsync(GetReservationByStartDate query);
    Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(GetReservationByEndDate query);
    
    Task<IEnumerable<Reservation>> GetReservationsByOwnerIdAsync(GetReservationsByOwnerIdQuery query);
    
}