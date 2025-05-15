using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Queries;

namespace LocalManagement.Domain.Services;

public interface ICommentQueryService
{
    Task<IEnumerable<Comment>> Handle(GetAllCommentsByLocalIdQuery query);
}