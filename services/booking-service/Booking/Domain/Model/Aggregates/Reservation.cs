using Booking.Domain.Model.Commands;

namespace Booking.Domain.Model.Aggregates;

public partial class Reservation
{
    public int Id { get; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; }
    public int LocalId { get; }
    
    
}

public partial class Reservation
{
    public Reservation()
    {
        UserId = 0;
        LocalId = 0;
    }
    
    public Reservation(CreateReservationCommand command)
    {
        StartDate = command.StartDate;
        EndDate = command.EndDate;
        UserId = command.UserId;
        LocalId = command.LocalId;
    }
    
    public void UpdateDate(UpdateReservationDateCommand command)
    {
        StartDate = command.StartDate;
        EndDate = command.EndDate;
    }
}