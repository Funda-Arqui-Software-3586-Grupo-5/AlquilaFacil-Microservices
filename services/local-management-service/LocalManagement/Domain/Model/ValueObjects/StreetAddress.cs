namespace LocalManagement.Domain.Model.ValueObjects;

public record StreetAddress(string District, string Street)
{
    public StreetAddress() : this(string.Empty, string.Empty)
    {
        
    }

    public StreetAddress(string province) : this(province, string.Empty)
    {
           
    }

    public string FullAddress => $"{District}, {Street}";
}