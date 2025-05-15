using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Commands;

namespace LocalManagement.Domain.Services;

public interface ICommentCommandService
{
    Task<Comment?> Handle(CreateCommentCommand command);
}