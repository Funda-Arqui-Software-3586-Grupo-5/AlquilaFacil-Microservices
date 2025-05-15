namespace AlquilaFacilPlatform.Profiles.Domain.Model.Commands;

public record UpdateProfileCommand(int Id, string Name, string? FatherName, string? MotherName, string DateOfBirth, string DocumentNumber,
    string Phone, int UserId, string PhotoUrl);