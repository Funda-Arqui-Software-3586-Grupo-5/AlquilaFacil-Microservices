namespace Booking.Interfaces.ACL.DTOs;

public class LocalDto
{
    public int Id { get; set; }
    public string LocalName { get; set; } = string.Empty;
    public int NightPrice { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string CityPlace { get; set; } = string.Empty;
    public string DescriptionMessage { get; set; } = string.Empty;
    public string Features { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int UserId { get; set; }
    public int LocalCategoryId { get; set; }
}