using System.Text;
using System.Text.Json;
using Notification.Domain.AMQP;
using Notification.Shared.Domain.Model.ValueObjects;
using RabbitMQ.Client;

namespace Notification.Application.Publishers;

public class MessagePublisher : IMessagePublisher
{
    private readonly IModel _channel;
    
    public MessagePublisher(RabbitMqChannelWrapper channelWrapper)
    {
        _channel = channelWrapper.Channel;
    }

    public async Task SendMessageAsync(object command)
    {
        try
        {
            string message = JsonSerializer.Serialize(command);
            var body = Encoding.UTF8.GetBytes(message);
            await Task.Run(() =>
            {
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true; 
                _channel.BasicPublish(exchange: "",
                    routingKey: _channel.CurrentQueue,
                    basicProperties: properties,
                    body: body);
            });
            
            Console.WriteLine($"Sent message: {message}");

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to start consuming messages.", ex);
        }
        
        
    }
}