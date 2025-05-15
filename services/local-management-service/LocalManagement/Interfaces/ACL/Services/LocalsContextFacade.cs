using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;

namespace LocalManagement.Interfaces.ACL.Services;

public class LocalsContextFacade(ILocalQueryService localQueryService) : ILocalsContextFacade
{

    public async Task<bool> LocalExists(int localId)
    {
        var query = new GetLocalByIdQuery(localId);
        var local = await localQueryService.Handle(query);
        return local != null;
    }

    public async Task<IEnumerable<Local?>> GetLocalsByUserId(int userId)
    {
        var query = new GetLocalsByUserIdQuery(userId);
        var locals = await localQueryService.Handle(query);
        return locals;
    }

    public async Task<bool> IsLocalOwner(int userId, int localId)
    {
        var query = new GetLocalByIdQuery(localId);
        var local = await localQueryService.Handle(query);
        if (local == null)
            return false;

        return local.UserId == userId;
    }
}