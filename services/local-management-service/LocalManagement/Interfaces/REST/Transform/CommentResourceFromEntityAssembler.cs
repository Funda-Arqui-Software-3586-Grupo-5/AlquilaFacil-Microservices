using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Interfaces.REST.Resources;

namespace LocalManagement.Interfaces.REST.Transform;

public static class CommentResourceFromEntityAssembler
{
    public static CommentResource ToResourceFromEntity(Comment entity)
    {
        return new CommentResource(entity.Id, entity.UserId, entity.LocalId, entity.CommentText, entity.CommentRating);
    }
}