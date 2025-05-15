namespace Booking.Interfaces.ACL.DTOs;

public class SubscriptionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SubscriptionStatusId { get; set; }
    public int PlanId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
}
