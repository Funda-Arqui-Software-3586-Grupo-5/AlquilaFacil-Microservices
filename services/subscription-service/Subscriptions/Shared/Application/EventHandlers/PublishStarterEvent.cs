using RabbitMQ.Client;

namespace Subscriptions.Shared.Application.EventHandlers;

public static class PublishStarterEvent
{
    
    public static void StartPublishing(this IServiceProvider serviceProvider, IConfiguration configuration, IModel channel)
    {
        var queue = configuration["Queue"];

        channel.QueueDeclare(
            queue: queue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        
    }
}