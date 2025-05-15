using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Repositories;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocalManagement.Infrastructure.Persistence.EFC.Repositories;

public class ReportRepository(AppDbContext context) : BaseRepository<Report>(context), IReportRepository
{
    public async Task<IEnumerable<Report>> GetReportsByLocalId(int localId)
    {
        return await Context.Set<Report>().Where(report => report.LocalId == localId).ToListAsync();
    }

    public async Task<IEnumerable<Report>> GetReportsByUserId(int userId)
    {
        return await Context.Set<Report>().Where(report => report.UserId == userId).ToListAsync();
    }
}