//using AlquilaFacilPlatform.Locals.Domain.Model.Aggregates;
using Booking.Interfaces.ACL;
using Booking.Interfaces.ACL.DTOs;

namespace Booking.Application.External;

public interface ILocalExternalService
{
    Task<bool> LocalReservationExists(int reservationId);
    Task<IEnumerable<LocalDto>> GetLocalsByUserId(int userId);
    Task<bool> IsLocalOwner(int userId, int localId);
}