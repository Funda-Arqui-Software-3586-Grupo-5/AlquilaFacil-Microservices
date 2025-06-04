using LocalManagement.Domain.AMQP;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Application.Internal.QueryServices;

public class LocalQueryService(ILocalRepository localRepository, IMessagePublisher messagePublisher) : ILocalQueryService
{
    public async Task<IEnumerable<Local>> Handle(GetAllLocalsQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await localRepository.ListAsync();
    }

    public async Task<Local?> Handle(GetLocalByIdQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await localRepository.FindByIdAsync(query.LocalId);
    }

    public async Task<HashSet<string>> Handle(GetAllLocalDistrictsQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return localRepository.GetAllDistrictsAsync();
    }

    public async Task<IEnumerable<Local>> Handle(GetLocalsByUserIdQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await localRepository.GetLocalsByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Local>> Handle(GetLocalsByCategoryIdAndCapacityRangeQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await localRepository.GetLocalsByCategoryIdAndCapacityrange(query.LocalCategoryId, query.MinCapacity,
            query.MaxCapacity);
    }

    public async Task<bool> Handle(IsLocalOwnerQuery query)
    {
        await messagePublisher.SendMessageAsync(query);
        return await localRepository.IsOwnerAsync(query.UserId, query.LocalId);
    }
}