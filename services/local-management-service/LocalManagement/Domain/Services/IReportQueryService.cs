using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Queries;

namespace LocalManagement.Domain.Services;

public interface IReportQueryService
{
    Task<IEnumerable<Report?>> Handle(GetReportsByLocalIdQuery query);
    Task<IEnumerable<Report?>> Handle(GetReportsByUserIdQuery query);
}