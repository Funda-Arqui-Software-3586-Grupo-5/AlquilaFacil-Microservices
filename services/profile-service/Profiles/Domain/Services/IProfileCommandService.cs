using AlquilaFacilPlatform.Profiles.Domain.Model.Commands;
using Profiles.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Profiles.Domain.Services;

public interface IProfileCommandService
{
    public Task<Profile?> Handle(CreateProfileCommand command);
    public Task<Profile> Handle(UpdateProfileCommand command);
}