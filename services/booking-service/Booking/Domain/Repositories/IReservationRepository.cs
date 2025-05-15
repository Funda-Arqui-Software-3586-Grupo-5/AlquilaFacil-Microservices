using System.Collections;
using Booking.Domain.Model.Aggregates;
using Booking.Shared.Domain.Repositories;

namespace Booking.Domain.Repositories;

public interface IReservationRepository : IBaseRepository<Booking.Domain.Model.Aggregates.Reservation>
{
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId);
    Task<IEnumerable<Reservation>>GetReservationByStartDateAsync(DateTime startDate);
    Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(DateTime endDate);
    Task<IEnumerable<Reservation>> GetReservationsByLocalIdAsync(List<int> localId);
}