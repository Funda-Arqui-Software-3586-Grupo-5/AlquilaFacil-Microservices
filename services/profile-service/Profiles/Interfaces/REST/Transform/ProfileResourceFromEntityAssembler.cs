using Profiles.Domain.Model.Aggregates;
using Profiles.Interfaces.REST.Resources;

namespace Profiles.Interfaces.REST.Transform;

public class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile entity)
    {
        return new ProfileResource(
            entity.Id, 
            entity.FullName, 
            entity.PhoneNumber, 
            entity.NumberDocument, 
            entity.BirthDate,entity.UserId, entity.PhotoUrl);
    }
}