using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;

namespace LocalManagement.Application.Internal.QueryServices;

public class ReportQueryService (IReportRepository reportRepository) : IReportQueryService
{
    public async Task<IEnumerable<Report?>> Handle(GetReportsByLocalIdQuery query)
    {
        return await reportRepository.GetReportsByLocalId(query.LocalId);
    }

    public async Task<IEnumerable<Report?>> Handle(GetReportsByUserIdQuery query)
    {
        return await reportRepository.GetReportsByUserId(query.UserId);
    }
}