using IAM.Domain.Model.Commands;
using IAM.Interfaces.REST.Resources;

namespace IAM.Interfaces.REST.Transform;

public static class UpdateUsernameCommandFromResourceAssembler
{
    public static UpdateUsernameCommand ToUpdateUsernameCommand(int id,UpdateUsernameResource resource)
    {
        return new UpdateUsernameCommand(id,resource.Username);
    }
}