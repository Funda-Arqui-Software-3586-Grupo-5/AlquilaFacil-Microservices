using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Interfaces.REST.Resources;

namespace LocalManagement.Interfaces.REST.Transform;

public class ReportResourceFromEntityAssembler
{
    public static ReportResource ToResourceFromEntity(Report? report)
    {
        return new ReportResource(
            report.Id,
            report.LocalId,
            report.Title,
            report.UserId,
            report.Description
        );
    }
}