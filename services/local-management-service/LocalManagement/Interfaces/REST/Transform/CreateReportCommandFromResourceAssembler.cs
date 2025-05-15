using LocalManagement.Domain.Model.Commands;
using LocalManagement.Interfaces.REST.Resources;

namespace LocalManagement.Interfaces.REST.Transform;

public class CreateReportCommandFromResourceAssembler
{
    public static CreateReportCommand ToCommandFromResource(CreateReportResource resource)
    {
        return new CreateReportCommand(
             resource.LocalId,
             resource.Title,
             resource.UserId,
             resource.Description
        );
    }
}