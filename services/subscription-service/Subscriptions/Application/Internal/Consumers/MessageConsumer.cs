namespace Subscriptions.Application.Internal.Consumers;
using System.Text;
using Subscriptions.Shared.Domain.Model.ValueObjects;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class MessageConsumer
{
    private readonly IModel _channel;

    public MessageConsumer(RabbitMqChannelWrapper channelWrapper)
    {
        _channel = channelWrapper.Channel;
    }

    public void StartConsuming(string queueName)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received message: {message}");
            await Task.Delay(100);


            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        Console.WriteLine($"Started consuming messages from queue: {queueName}");
    }
}