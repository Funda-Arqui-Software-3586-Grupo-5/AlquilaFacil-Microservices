using LocalManagement.Domain.Model.Entities;
using LocalManagement.Domain.Model.ValueObjects;
using LocalManagement.Domain.Repositories;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocalManagement.Infrastructure.Persistence.EFC.Repositories;

public class LocalCategoryRepository(AppDbContext context)
    : BaseRepository<LocalCategory>(context), ILocalCategoryRepository
{
    public Task<bool> ExistsLocalCategory(EALocalCategoryTypes type)
    {
        return Context.Set<LocalCategory>().AnyAsync(x => x.Name == type.ToString());
    }

    public async Task<IEnumerable<LocalCategory>> GetAllLocalCategories()
    {
        return await Context.Set<LocalCategory>().ToListAsync();
    }
}
