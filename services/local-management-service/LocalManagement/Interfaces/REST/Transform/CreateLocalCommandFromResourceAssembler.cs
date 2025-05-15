using LocalManagement.Domain.Model.Commands;
using LocalManagement.Interfaces.REST.Resources;

namespace LocalManagement.Interfaces.REST.Transform;

public static class CreateLocalCommandFromResourceAssembler
{
    public static CreateLocalCommand ToCommandFromResources(CreateLocalResource resource)
    {
        return new CreateLocalCommand(resource.District, resource.Street, resource.LocalName, resource.Country, resource.City, resource.Price,
            resource.PhotoUrl, resource.DescriptionMessage, resource.LocalCategoryId,resource.UserId,resource.Features,resource.Capacity);
    }
}