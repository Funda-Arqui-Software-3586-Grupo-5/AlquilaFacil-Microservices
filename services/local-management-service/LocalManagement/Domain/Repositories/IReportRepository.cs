using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Shared.Domain.Repositories;

namespace LocalManagement.Domain.Repositories;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<IEnumerable<Report>> GetReportsByLocalId(int localId);
    Task<IEnumerable<Report>> GetReportsByUserId(int userId);
}