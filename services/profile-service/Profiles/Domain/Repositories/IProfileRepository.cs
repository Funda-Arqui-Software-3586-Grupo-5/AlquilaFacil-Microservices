using AlquilaFacilPlatform.Shared.Domain.Repositories;
using Profiles.Domain.Model.Aggregates;

namespace AlquilaFacilPlatform.Profiles.Domain.Repositories;

public interface IProfileRepository : IBaseRepository<Profile>
{
    Task<Profile?> FindByUserIdAsync(int userId);
}