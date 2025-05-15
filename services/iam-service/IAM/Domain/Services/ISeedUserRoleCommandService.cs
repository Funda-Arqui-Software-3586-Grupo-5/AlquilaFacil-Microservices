using IAM.Domain.Model.Commands;

namespace IAM.Domain.Services;

public interface ISeedUserRoleCommandService
{
    Task Handle(SeedUserRolesCommand command);
}