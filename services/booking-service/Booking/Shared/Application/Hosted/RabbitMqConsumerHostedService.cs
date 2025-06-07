using Booking.Application.Internal.Consumers;

namespace Booking.Shared.Application.Hosted;

public class RabbitMqConsumerHostedService : IHostedService
{
    private readonly MessageConsumer _messageConsumer;
    private readonly IConfiguration _configuration;

    public RabbitMqConsumerHostedService(MessageConsumer messageConsumer, IConfiguration configuration)
    {
        _messageConsumer = messageConsumer;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var queueName = _configuration["Queue"];
        _messageConsumer.StartConsuming(queueName);
        Console.WriteLine($"RabbitMQ consumer started for queue: {queueName}");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Add cleanup logic if needed
        return Task.CompletedTask;
    }
}