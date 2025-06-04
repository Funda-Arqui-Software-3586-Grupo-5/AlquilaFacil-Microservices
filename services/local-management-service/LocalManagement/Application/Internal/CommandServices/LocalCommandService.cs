using LocalManagement.Application.External.OutboundServices;
using LocalManagement.Domain.AMQP;
using LocalManagement.Domain.Model.Commands;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;
using LocalManagement.Shared.Domain.Repositories;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Application.Internal.CommandServices;

public class LocalCommandService (IUserExternalService userCommentExternalService, ILocalRepository localRepository, ILocalCategoryRepository localCategoryRepository, IUnitOfWork unitOfWork, IMessagePublisher messagePublisher) : ILocalCommandService
{
    
    public async Task<Local?> Handle(CreateLocalCommand command)
    {
        var localCategory = await localCategoryRepository.FindByIdAsync(command.LocalCategoryId);
        var userExists = await userCommentExternalService.UserExists(command.UserId);
        if (!userExists)
        {
            throw new Exception("User does not exist");
        }
        if (localCategory == null)
        {
            throw new Exception("Local category not found");
        }

        if (command.Price <= 0)
        {
            throw new Exception("Price must be greater than 0");
        }
        var local = new Local(command);
        await localRepository.AddAsync(local);
        await messagePublisher.SendMessageAsync(command);
        await unitOfWork.CompleteAsync();
        return local;
    }

    public async Task<Local?> Handle(UpdateLocalCommand command)
    {
        var local = await localRepository.FindByIdAsync(command.Id);
        if (local == null)
        {
            throw new Exception("Local not found");
        }
        var localCategory = await localCategoryRepository.FindByIdAsync(command.LocalCategoryId);
        if (localCategory == null)
        {
            throw new Exception("Local category not found");
        }

        if (command.Price <= 0)
        {
            throw new Exception("Price must be greater than 0");
        }
        localRepository.Update(local);
        local.Update(command);
        await messagePublisher.SendMessageAsync(command);
        await unitOfWork.CompleteAsync();
        return local;
    }
}