namespace LocalManagement.Domain.AMQP;

public interface IMessagePublisher
{
    Task SendMessageAsync(object message);
}