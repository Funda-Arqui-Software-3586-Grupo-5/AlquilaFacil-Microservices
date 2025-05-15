using LocalManagement.Shared.Domain.Repositories;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace LocalManagement.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context) => _context = context;
    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}