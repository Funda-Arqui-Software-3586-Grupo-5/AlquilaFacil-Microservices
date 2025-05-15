using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;

namespace LocalManagement.Application.Internal.QueryServices;

public class CommentQueryService (ICommentRepository commentRepository) : ICommentQueryService
{
    public Task<IEnumerable<Comment>> Handle(GetAllCommentsByLocalIdQuery query)
    {
        return commentRepository.GetAllCommentsByLocalId(query.LocalId);
    }
}