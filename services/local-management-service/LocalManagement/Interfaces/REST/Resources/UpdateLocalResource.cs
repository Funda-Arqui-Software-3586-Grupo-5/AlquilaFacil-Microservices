namespace LocalManagement.Interfaces.REST.Resources;

public record UpdateLocalResource(string District, string Street, string LocalName, string Country, string City, 
    int Price, string PhotoUrl, string DescriptionMessage, int LocalCategoryId, int UserId,string Features,int Capacity);