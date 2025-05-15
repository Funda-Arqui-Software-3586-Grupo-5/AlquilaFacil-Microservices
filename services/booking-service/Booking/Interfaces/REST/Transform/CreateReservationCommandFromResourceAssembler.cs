using Booking.Domain.Model.Commands;
using Booking.Interfaces.REST.Resources;

namespace Booking.Interfaces.REST.Transform;

public static class CreateReservationCommandFromResourceAssembler
{
    public static CreateReservationCommand ToCommandFromResource( CreateReservationResource resource)
    {
        return new CreateReservationCommand(
            resource.StartDate,
            resource.EndDate,
            resource.UserId,
            resource.LocalId
        );
    }
}