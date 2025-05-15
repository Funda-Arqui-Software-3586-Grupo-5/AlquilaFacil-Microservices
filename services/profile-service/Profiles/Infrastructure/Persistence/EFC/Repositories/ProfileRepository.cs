using AlquilaFacilPlatform.Profiles.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Model.Aggregates;
using Profiles.Shared.Infrastructure.Persistence.EFC.Configuration;
using Profiles.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Profiles.Infrastructure.Persistence.EFC.Repositories;


public class ProfileRepository(AppDbContext context) : BaseRepository<Profile>(context), IProfileRepository
{
    public async Task<Profile?> FindByUserIdAsync(int userId)
    {
        return await context.Set<Profile>().FirstOrDefaultAsync(p => p.UserId == userId);
    }
}