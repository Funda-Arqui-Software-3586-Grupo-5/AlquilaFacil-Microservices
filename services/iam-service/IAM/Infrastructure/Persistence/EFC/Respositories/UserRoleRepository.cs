using IAM.Domain.Model.Entities;
using IAM.Domain.Model.ValueObjects;
using IAM.Domain.Respositories;
using IAM.Shared.Infrastructure.Persistence.EFC.Configuration;
using IAM.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IAM.Infrastructure.Persistence.EFC.Respositories;

public class UserRoleRepository(AppDbContext context) : BaseRepository<UserRole>(context), IUserRoleRepository
{
    public async Task<bool> ExistsUserRole(EUserRoles role)
    {
        return await Context.Set<UserRole>().AnyAsync(userRole => userRole.Role == role.ToString());
    }
}
