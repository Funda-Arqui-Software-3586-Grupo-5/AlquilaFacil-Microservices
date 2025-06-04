using LocalManagement.Domain.AMQP;
using LocalManagement.Domain.Model.Entities;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;

namespace LocalManagement.Application.Internal.QueryServices;

public class LocalCategoryQueryService(ILocalCategoryRepository localCategoryRepository, IMessagePublisher messagePublisher) : ILocalCategoryQueryService
{
    public async Task<IEnumerable<LocalCategory>> Handle(GetAllLocalCategoriesQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await localCategoryRepository.GetAllLocalCategories();
    }
}