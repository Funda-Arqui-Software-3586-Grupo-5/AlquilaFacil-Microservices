using LocalManagement.Domain.Repositories;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Infrastructure.Persistence.EFC.Repositories;

public class LocalRepository(AppDbContext context) : BaseRepository<Local>(context), ILocalRepository
{

    public HashSet<string> GetAllDistrictsAsync()
    {

        var placeInfo = context.Set<Local>().Select(x => " " + x.Place.City + ", " +  x.Place.Country).Distinct();
        var districtsInfo = context.Set<Local>().Select(x => x.StreetAddress).ToList();
        var districts = new HashSet<string>();
        foreach (var place in placeInfo)
        {
            foreach (var district in districtsInfo)
            {
                var districtName = district.Split(",")[0];
                districts.Add(districtName + "," + place);
            }
        }

        return districts;
    }

    public async Task<IEnumerable<Local>> GetLocalsByCategoryIdAndCapacityrange(int categoryId, int minCapacity, int maxCapacity)
    {
        return await context.Set<Local>().Where(x => x.LocalCategoryId == categoryId && x.Capacity >= minCapacity && x.Capacity <= maxCapacity).ToListAsync();
    }

    public async Task<IEnumerable<Local>> GetLocalsByUserIdAsync(int userId)
    {
        return await context.Set<Local>().Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<bool> IsOwnerAsync(int userId, int localId)
    {
        return await context.Set<Local>().AnyAsync(x => x.UserId == userId && x.Id == localId);
    }

    public async Task<Local?> GetLocalByUserId(int userId, int localId)
    {
        return await context.Set<Local>().FirstOrDefaultAsync(x => x.UserId == userId && x.Id == localId);
    }
}
