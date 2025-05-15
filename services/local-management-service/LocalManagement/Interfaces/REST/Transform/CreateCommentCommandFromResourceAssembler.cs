using LocalManagement.Domain.Model.Commands;
using LocalManagement.Interfaces.REST.Resources;

namespace LocalManagement.Interfaces.REST.Transform;

public static class CreateCommentCommandFromResourceAssembler
{
    public static CreateCommentCommand ToCommandFromResource(CreateCommentResource resource)
    {
        return new CreateCommentCommand(resource.UserId, resource.LocalId, resource.Text, resource.Rating);
    }
}