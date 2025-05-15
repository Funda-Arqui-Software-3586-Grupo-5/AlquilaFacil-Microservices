using LocalManagement.Domain.Model.Entities;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;

namespace LocalManagement.Application.Internal.QueryServices;

public class LocalCategoryQueryService(ILocalCategoryRepository localCategoryRepository) : ILocalCategoryQueryService
{
    public async Task<IEnumerable<LocalCategory>> Handle(GetAllLocalCategoriesQuery query)
    {
        return await localCategoryRepository.GetAllLocalCategories();
    }
}