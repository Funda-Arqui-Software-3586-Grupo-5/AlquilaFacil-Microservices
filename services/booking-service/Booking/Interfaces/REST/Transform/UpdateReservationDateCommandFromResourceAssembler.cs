using Booking.Domain.Model.Commands;
using Booking.Interfaces.REST.Resources;

namespace Booking.Interfaces.REST.Transform;

public static class UpdateReservationDateCommandFromResourceAssembler
{
    public static UpdateReservationDateCommand ToCommandFromResource(int id,UpdateReservationResource resource)
    {
        return new UpdateReservationDateCommand(
            id,
            resource.StartDate,
            resource.EndDate
        );
    }
}

