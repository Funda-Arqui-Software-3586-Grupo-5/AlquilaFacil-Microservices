//using AlquilaFacilPlatform.Subscriptions.Domain.Model.Aggregates;
//using AlquilaFacilPlatform.Subscriptions.Interfaces.ACL;

using Booking.Interfaces.ACL;
using Booking.Interfaces.ACL.DTOs;

namespace Booking.Application.External.OutboundServices;

public class SubscriptionExternalService(ISubscriptionContextFacade subscriptionContextFacade)
    : ISubscriptionExternalService
{
    public async Task<IEnumerable<SubscriptionDto>> GetSubscriptionByUsersId(List<int> usersId)
    {
        return await subscriptionContextFacade.GetSubscriptionsByUsersId(usersId);
    }
}