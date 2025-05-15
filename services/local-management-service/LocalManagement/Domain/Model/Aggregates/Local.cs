using LocalManagement.Domain.Model.Commands;
using LocalManagement.Domain.Model.ValueObjects;

namespace LocalManagement.Domain.Model.Aggregates;

public partial class Local
{
    public Local()
    {
        LName = new LocalName();
        Address = new StreetAddress();
        Price = new NightPrice();
        Photo = new PhotoUrl();
        Place = new CityPlace();
        Description = new DescriptionMessage();
        Features = string.Empty;
        Capacity = 0;
        UserId = 0;
    }
    
    
    public Local(string district, string street, string localType, string country, string city, int price, 
           string photoUrl, string descriptionMessage , int localCategoryId,int userId, string features, int capacity) : this()
    {
        LName = new LocalName(localType);
        Address = new StreetAddress(district, street);
        Price = new NightPrice(price);
        Photo = new PhotoUrl(photoUrl);
        Place = new CityPlace(country, city);
        Description = new DescriptionMessage(descriptionMessage);
        LocalCategoryId = localCategoryId;
        UserId = userId;
        Features = features;
        Capacity = capacity;
        
    }

    public Local(CreateLocalCommand command)
    {
        LName = new LocalName(command.LocalType);
        Address = new StreetAddress(command.District, command.Street);
        Price = new NightPrice(command.Price);
        Description = new DescriptionMessage(command.DescriptionMessage);
        Photo = new PhotoUrl(command.PhotoUrl);
        Place = new CityPlace(command.Country, command.City);
        LocalCategoryId = command.LocalCategoryId;
        Features = command.Features;
        Capacity = command.Capacity;
        UserId = command.UserId;
    }


    public void Update(UpdateLocalCommand command)
    {
        LName = new LocalName(command.LocalType);
        Address = new StreetAddress(command.District, command.Street);
        Price = new NightPrice(command.Price);
        Description = new DescriptionMessage(command.DescriptionMessage);
        Photo = new PhotoUrl(command.PhotoUrl);
        Place = new CityPlace(command.Country, command.City);
        LocalCategoryId = command.LocalCategoryId;
        Features = command.Features;
        Capacity = command.Capacity;
        UserId = command.UserId;
    }

    public int Id { get; }
    
    public string Features { get; set; }
    
    public int Capacity { get; set; }
    public LocalName LName { get; private set; }
    public NightPrice Price { get; private set; }
    public PhotoUrl Photo { get; private set; }
    public StreetAddress Address { get; private set; }
    public CityPlace Place { get; private set; }
    public DescriptionMessage Description { get; private set; }
    public int LocalCategoryId { get; set; }
    public int UserId { get; set; }

    
    public string StreetAddress => Address.FullAddress;
    public string LocalName => LName.TypeLocal;
    public int NightPrice => Price.PriceNight;
    public string PhotoUrl => Photo.PhotoUrlLink;
    public string CityPlace => Place.FullCityPlace;
    public string DescriptionMessage => Description.MessageDescription;
}