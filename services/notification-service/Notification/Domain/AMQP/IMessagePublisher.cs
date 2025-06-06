namespace Notification.Domain.AMQP;

public interface IMessagePublisher
{
    Task SendMessageAsync(object message);
}