namespace LocalManagement.Domain.Model.ValueObjects;

public record CityPlace(string Country, string City)
{
    public CityPlace() : this(string.Empty, string.Empty)
    {
        
    }

    public CityPlace(string country) : this(country, string.Empty)
    {
        
    }

    public string FullCityPlace => $"{Country}, {City}";
}