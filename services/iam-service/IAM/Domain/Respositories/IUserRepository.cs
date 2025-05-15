using IAM.Domain.Model.Aggregates;
using IAM.Shared.Domain.Repositories;

namespace IAM.Domain.Respositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync (string email);
    bool ExistsByUsername(string username);
    
    Task<string?> GetUsernameByIdAsync(int userId);
    
    bool ExistsById(int userId);
}