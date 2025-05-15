using Booking.Domain.Model.Commands;
using Booking.Interfaces.REST.Resources;

namespace Booking.Interfaces.REST.Transform;

public static class DeleteReservationCommandFromResourceAssembler
{
    public static DeleteReservationCommand ToCommandFromResource(DeleteReservationResource resource)
    {
        return new DeleteReservationCommand(resource.Id);
    }
}