using AlquilaFacilPlatform.Profiles.Domain.Model.Commands;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Profiles.Interfaces.REST.Transform;

public class UpdateProfileCommandFromResourceAssembler
{
    public static UpdateProfileCommand ToCommandFromResource(UpdateProfileResource resource, int id)
    {
        return new UpdateProfileCommand(id,resource.Name, resource.FatherName, resource.MotherName, resource.DateOfBirth,
            resource.DocumentNumber, resource.Phone, resource.UserId, resource.PhotoUrl);
    }
}