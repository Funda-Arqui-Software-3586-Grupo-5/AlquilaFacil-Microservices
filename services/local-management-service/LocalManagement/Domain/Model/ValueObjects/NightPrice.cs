namespace LocalManagement.Domain.Model.ValueObjects;

public record NightPrice(int PriceNight)
{
    public NightPrice() : this(0)
    {
        
    }
}