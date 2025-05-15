using AlquilaFacilPlatform.Profiles.Domain.Model.Commands;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Resources;

namespace AlquilaFacilPlatform.Profiles.Interfaces.REST.Transform;

public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        return new CreateProfileCommand(resource.Name, resource.FatherName, resource.MotherName, resource.DateOfBirth,
            resource.DocumentNumber, resource.Phone, resource.UserId, resource.PhotoUrl);
    }
}