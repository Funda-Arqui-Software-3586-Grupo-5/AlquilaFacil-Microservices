using LocalManagement.Domain.AMQP;
using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;

namespace LocalManagement.Application.Internal.QueryServices;

public class CommentQueryService (ICommentRepository commentRepository, IMessagePublisher messagePublisher) : ICommentQueryService
{
    public async Task<IEnumerable<Comment>> Handle(GetAllCommentsByLocalIdQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await commentRepository.GetAllCommentsByLocalId(query.LocalId);
    }
}