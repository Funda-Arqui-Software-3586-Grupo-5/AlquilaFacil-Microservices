using IAM.Domain.Model.Commands;
using IAM.Domain.Model.Entities;
using IAM.Domain.Model.ValueObjects;
using IAM.Domain.Respositories;
using IAM.Domain.Services;
using IAM.Shared.Domain.Repositories;

namespace IAM.Application.Internal.CommandServices;

public class SeedUserRoleCommandService(IUserRoleRepository repository, IUnitOfWork unitOfWork) : ISeedUserRoleCommandService
{
    public async Task Handle(SeedUserRolesCommand command)
    {
        foreach (EUserRoles role in Enum.GetValues(typeof(EUserRoles)))
        {
            if (await repository.ExistsUserRole(role)) continue;
            var userRole = new UserRole(role.ToString());
            await repository.AddAsync(userRole);
        }

        await unitOfWork.CompleteAsync();
    }
}