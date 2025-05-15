namespace LocalManagement.Interfaces.REST.Resources;

public record LocalResource(int Id, string StreetAddress, string LocalName, string CityPlace, int NightPrice, 
    string PhotoUrl, string DescriptionMessage, int LocalCategoryId, int UserId, string Features,int Capacity);