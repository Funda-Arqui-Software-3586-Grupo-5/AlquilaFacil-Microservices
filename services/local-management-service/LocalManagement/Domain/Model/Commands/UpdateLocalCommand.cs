namespace LocalManagement.Domain.Model.Commands;

public record UpdateLocalCommand(int Id, string District, string Street, string LocalType, string Country, string City, int Price, string PhotoUrl, string DescriptionMessage, int LocalCategoryId, int UserId, string Features,int Capacity);