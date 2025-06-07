

using RabbitMQ.Client;

namespace Booking.Shared.Domain.Model.ValueObjects;

public class RabbitMqChannelWrapper
{
    public IModel Channel { get; set; }

    public RabbitMqChannelWrapper(IModel channel)
    {
        Channel = channel;
    }
}