namespace LocalManagement.Domain.Model.ValueObjects;

public record RatingComment(int Rating)
{
    public RatingComment() : this(0)
    {
        
    }
}