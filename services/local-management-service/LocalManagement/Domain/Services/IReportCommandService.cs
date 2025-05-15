using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Commands;

namespace LocalManagement.Domain.Services;

public interface IReportCommandService
{
    Task<Report?> Handle(CreateReportCommand command);
    Task<Report?> Handle(DeleteReportCommand command);
}