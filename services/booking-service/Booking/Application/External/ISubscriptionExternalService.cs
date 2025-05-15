//using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;

using Booking.Interfaces.ACL;
using Booking.Interfaces.ACL.DTOs;

namespace Booking.Application.External;

public interface ISubscriptionExternalService
{
    Task<IEnumerable<SubscriptionDto>> GetSubscriptionByUsersId(List<int> usersId);
}