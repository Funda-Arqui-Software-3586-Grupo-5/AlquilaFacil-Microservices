using LocalManagement.Domain.Model.Aggregates;

namespace LocalManagement.Interfaces.ACL;

public interface ILocalsContextFacade
{

    Task<bool> LocalExists(int localId);
    
    Task<IEnumerable<Local?>> GetLocalsByUserId(int userId);
    
    Task<bool> IsLocalOwner(int userId, int localId);
}