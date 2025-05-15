using LocalManagement.Shared.Domain.Repositories;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Domain.Repositories;

public interface ILocalRepository : IBaseRepository<Local>
{
   HashSet<string> GetAllDistrictsAsync();
   
   Task<IEnumerable<Local>> GetLocalsByCategoryIdAndCapacityrange(int categoryId, int minCapacity, int maxCapacity);
   
   Task<IEnumerable<Local>> GetLocalsByUserIdAsync(int userId);
   Task<bool> IsOwnerAsync(int userId, int localId);
   
}