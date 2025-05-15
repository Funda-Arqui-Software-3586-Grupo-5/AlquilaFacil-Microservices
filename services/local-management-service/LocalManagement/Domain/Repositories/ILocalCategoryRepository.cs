using LocalManagement.Domain.Model.Entities;
using LocalManagement.Domain.Model.ValueObjects;
using LocalManagement.Shared.Domain.Repositories;

namespace LocalManagement.Domain.Repositories;

public interface ILocalCategoryRepository : IBaseRepository<LocalCategory>
{
    Task<bool> ExistsLocalCategory(EALocalCategoryTypes type);
    Task<IEnumerable<LocalCategory>> GetAllLocalCategories();
}