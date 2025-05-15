using LocalManagement.Domain.Model.Commands;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Domain.Services;

public interface ILocalCommandService
{
    Task<Local?> Handle(CreateLocalCommand command);
    Task<Local?> Handle(UpdateLocalCommand command);
}