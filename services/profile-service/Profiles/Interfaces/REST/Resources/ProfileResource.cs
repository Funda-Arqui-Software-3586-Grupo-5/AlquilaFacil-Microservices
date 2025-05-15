namespace Profiles.Interfaces.REST.Resources;

public record ProfileResource(int Id, string FullName, string Phone, string DocumentNumber, string DateOfBirth,int UserId, string PhotoUrl);