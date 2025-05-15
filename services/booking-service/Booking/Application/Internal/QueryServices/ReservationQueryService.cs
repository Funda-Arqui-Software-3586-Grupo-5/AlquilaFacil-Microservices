using Booking.Application.External;
using Booking.Application.External.OutboundServices;
using Booking.Domain.Model.Aggregates;
using Booking.Domain.Model.Queries;
using Booking.Domain.Repositories;
using Booking.Domain.Services;

namespace Booking.Application.Internal.QueryServices;

public class ReservationQueryService(IReservationRepository reservationRepository, ILocalExternalService localExternalService) : IReservationQueryService
{
    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(GetReservationsByUserId query)
    {
        return await reservationRepository.GetReservationsByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByStartDateAsync(GetReservationByStartDate query)
    {
        return await reservationRepository.GetReservationByStartDateAsync(query.StartDate);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByOwnerIdAsync(GetReservationsByOwnerIdQuery query)
    {
        
        var locals = await localExternalService.GetLocalsByUserId(query.OwnerId);
        if (locals == null)
        {
            throw new Exception("This user does not have any local registered.");
        }
        var localIds = locals.Select(local => local.Id);
        return await reservationRepository.GetReservationsByLocalIdAsync(localIds.ToList());
        
    }

    public async Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(GetReservationByEndDate query)
    {
        return await reservationRepository.GetReservationByEndDateAsync(query.EndDate);
    }
}