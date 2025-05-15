//using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
//using AlquilaFacilPlatform.Locals.Interfaces.ACL;

using Booking.Interfaces.ACL;
using Booking.Interfaces.ACL.DTOs;

namespace Booking.Application.External.OutboundServices;

public class LocalExternalService(ILocalsContextFacade _localsContextFacade) : ILocalExternalService
{
    public async Task<bool> LocalReservationExists(int reservationId) =>
        await _localsContextFacade.LocalExists(reservationId);

    public async Task<IEnumerable<LocalDto>> GetLocalsByUserId(int userId) =>
        await _localsContextFacade.GetLocalsByUserId(userId);

    public async Task<bool> IsLocalOwner(int userId, int localId) =>
        await _localsContextFacade.IsLocalOwner(userId, localId);
}
