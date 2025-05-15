using IAM.Domain.Model.Entities;
using IAM.Domain.Model.ValueObjects;
using IAM.Shared.Domain.Repositories;

namespace IAM.Domain.Respositories;

public interface IUserRoleRepository : IBaseRepository<UserRole>
{
    Task<bool> ExistsUserRole(EUserRoles role);
}