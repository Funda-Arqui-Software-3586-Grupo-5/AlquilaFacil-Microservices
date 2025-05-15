using LocalManagement.Application.External.OutboundServices;
using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Commands;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;
using LocalManagement.Shared.Domain.Repositories;

namespace LocalManagement.Application.Internal.CommandServices;

public class CommentCommandService(ICommentRepository commentRepository, ILocalRepository localRepository, IUserCommentExternalService userCommentExternalService, IUnitOfWork unitOfWork) : ICommentCommandService
{
    public async Task<Comment?> Handle(CreateCommentCommand command)
    {
        var local = await localRepository.FindByIdAsync(command.LocalId);
        var isUserExists = await userCommentExternalService.UserExists(command.UserId);
        if (local == null)
        {
            throw new Exception("There is no locals matching the id specified");
        }
        
        if (!isUserExists)
        {
            throw new Exception("There are no users matching the id specified");
        }
        
        if (command.Rating is > 5 or < 0)
        {
            throw new Exception("Rating needs to be a number between 0 and 5");
        }

        var comment = new Comment(command);
        await commentRepository.AddAsync(comment);
        await unitOfWork.CompleteAsync();
        return comment;
    }
}