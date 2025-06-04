using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;
using AlquilaFacilPlatform.Profiles.Domain.Repositories;
using AlquilaFacilPlatform.Profiles.Domain.Services;
using Profiles.Application.External.OutboundServices;
using Profiles.Domain.AMQP;
using Profiles.Domain.Model.Aggregates;

namespace Profiles.Application.Internal.QueryServices;

public class ProfileQueryService(IProfileRepository profileRepository, ISubscriptionExternalService subscriptionExternalService, IMessagePublisher messagePublisher) : IProfileQueryService
{
    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await profileRepository.ListAsync();
    }

    public async Task<Profile?> Handle(GetProfileByIdQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await profileRepository.FindByIdAsync(query.ProfileId);
    }

    public async Task<Profile?> Handle(GetProfileByUserIdQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await profileRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<bool> Handle(IsUserSubscribeQuery query)
    {
    await messagePublisher.SendMessageAsync(query);
        return await subscriptionExternalService.IsUserSubscribeAsync(query.Id);
    }
}