using Booking.Domain.Model.Aggregates;
using Booking.Domain.Repositories;
using Booking.Shared.Infrastructure.Persistence.EFC.Configuration;
using Booking.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Persistence.EFC.Repositories;

public class ReservationRepository(AppDbContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
    {
        return await Context.Set<Reservation>().Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationByStartDateAsync(DateTime startDate)
    {
        return await Context.Set<Reservation>().Where(r => r.StartDate == startDate).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(DateTime endDate)
    {
           return await Context.Set<Reservation>().Where(r => r.EndDate == endDate).ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByLocalIdAsync(List<int> localId)
    {
        return await Context.Set<Reservation>().Where(r => localId.Contains(r.LocalId)).ToListAsync();
    }
}