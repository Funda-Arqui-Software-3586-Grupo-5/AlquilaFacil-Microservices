using LocalManagement.Interfaces.REST.Resources;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Interfaces.REST.Transform;

public static class LocalResourceFromEntityAssembler
{
    public static LocalResource ToResourceFromEntity(Local local)
    {
        return new LocalResource(
            local.Id, 
            local.StreetAddress, 
            local.LocalName,
            local.CityPlace,
            local.NightPrice, 
            local.PhotoUrl,
            local.DescriptionMessage,
            local.LocalCategoryId,
            local.UserId,
            local.Features,
            local.Capacity
            );
    }
}