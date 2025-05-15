namespace LocalManagement.Domain.Model.ValueObjects;

public record LocalName(string TypeLocal)
{
    public LocalName() : this(String.Empty)
    {
        
    }
}