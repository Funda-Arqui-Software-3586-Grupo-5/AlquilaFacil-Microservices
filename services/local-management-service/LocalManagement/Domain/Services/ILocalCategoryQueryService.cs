using LocalManagement.Domain.Model.Entities;
using LocalManagement.Domain.Model.Queries;

namespace LocalManagement.Domain.Services;

public interface ILocalCategoryQueryService
{
    Task<IEnumerable<LocalCategory>> Handle(GetAllLocalCategoriesQuery query);
}
