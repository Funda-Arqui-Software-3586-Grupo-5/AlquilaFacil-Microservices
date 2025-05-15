using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Application.Internal.QueryServices;

public class LocalQueryService(ILocalRepository localRepository) : ILocalQueryService
{
    public async Task<IEnumerable<Local>> Handle(GetAllLocalsQuery query)
    {
        return await localRepository.ListAsync();
    }

    public async Task<Local?> Handle(GetLocalByIdQuery query)
    {
        return await localRepository.FindByIdAsync(query.LocalId);
    }

    public HashSet<string> Handle(GetAllLocalDistrictsQuery query)
    {
        return localRepository.GetAllDistrictsAsync();
    }

    public async Task<IEnumerable<Local>> Handle(GetLocalsByUserIdQuery query)
    {
        return await localRepository.GetLocalsByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Local>> Handle(GetLocalsByCategoryIdAndCapacityRangeQuery query)
    {
        return await localRepository.GetLocalsByCategoryIdAndCapacityrange(query.LocalCategoryId, query.MinCapacity,
            query.MaxCapacity);
    }

    public async Task<bool> Handle(IsLocalOwnerQuery query)
    {
        return await localRepository.IsOwnerAsync(query.UserId, query.LocalId);
    }
}