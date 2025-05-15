using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Repositories;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocalManagement.Infrastructure.Persistence.EFC.Repositories;

public class CommentRepository(AppDbContext context)
    : BaseRepository<Comment>(context), ICommentRepository
{
    public async Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId)
    {
        return await Context.Set<Comment>().Where(c => c.LocalId == localId).ToListAsync();
    }
}