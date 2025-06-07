namespace Booking.Domain.AMQP;

public interface IMessagePublisher
{
    Task SendMessageAsync(object message);
}