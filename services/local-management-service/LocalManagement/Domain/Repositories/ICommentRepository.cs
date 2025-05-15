using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Shared.Domain.Repositories;

namespace LocalManagement.Domain.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
        Task<IEnumerable<Comment>> GetAllCommentsByLocalId(int localId);
}