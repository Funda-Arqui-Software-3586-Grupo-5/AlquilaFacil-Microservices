namespace Booking.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}