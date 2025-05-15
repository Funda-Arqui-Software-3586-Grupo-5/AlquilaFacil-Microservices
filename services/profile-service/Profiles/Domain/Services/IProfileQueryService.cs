using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;
using Profiles.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Profiles.Domain.Services;

public interface IProfileQueryService
{
    Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query);
    Task<Profile?> Handle(GetProfileByIdQuery query);

    Task<Profile?> Handle(GetProfileByUserIdQuery query);
    Task<bool> Handle(IsUserSubscribeQuery query);
}