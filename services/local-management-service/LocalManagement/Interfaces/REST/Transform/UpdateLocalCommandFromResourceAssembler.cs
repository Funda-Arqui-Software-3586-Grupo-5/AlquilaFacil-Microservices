using LocalManagement.Domain.Model.Commands;
using LocalManagement.Interfaces.REST.Resources;

namespace LocalManagement.Interfaces.REST.Transform;

public static class UpdateLocalCommandFromResourceAssembler
{
    public static UpdateLocalCommand ToCommandFromResource(int id,UpdateLocalResource resource)
    {
        return new UpdateLocalCommand(id,resource.District, resource.Street, resource.LocalName, resource.Country, resource.City, resource.Price,
            resource.PhotoUrl, resource.DescriptionMessage, resource.LocalCategoryId, resource.UserId,resource.Features,resource.Capacity);
    }
}