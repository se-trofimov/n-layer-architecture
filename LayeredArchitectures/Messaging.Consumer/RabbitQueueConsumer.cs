using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mime;
using System.Text.Json;
using Messaging.Abstractions;
using Microsoft.Extensions.Logging;

namespace Messaging.Consumer;
public class RabbitQueueConsumer : IQueueConsumer
{
    private readonly ILogger _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitQueueConsumer(IConnectionFactory connectionFactory, ILogger<RabbitQueueConsumer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        IConnectionFactory connectionFactory1 = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _connection = connectionFactory1.CreateConnection();
        _channel = _connection.CreateModel();
    }
    public Task ListenAsync<T>(string destination, Func<T, Task> onMessageReceived, CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            _logger.LogInformation("Message is received. Destination {destination}. ContentType {type}. Delivery tag {tag}",
                destination, ea.BasicProperties.ContentType, ea.DeliveryTag);

            if (ea.BasicProperties.ContentType != MediaTypeNames.Application.Json)
            {
                _channel.BasicAck(ea.DeliveryTag, false);
                return;
            }
            
            try
            {
                var body = ea.Body.Span;
                var deserialized = JsonSerializer.Deserialize<T>(body);

                if (deserialized is { })
                    onMessageReceived(deserialized);

                _channel.BasicAck(ea.DeliveryTag, false);

                _logger.LogInformation("Message is acknowledged. Destination {destination}. ContentType {type}. Delivery tag {tag}",
                    destination, ea.BasicProperties.ContentType, ea.DeliveryTag);
            }
            catch (Exception e)
            {
                _logger.LogError("Unable process a message. Destination {destination}. Exception {exception}", 
                    destination, e);
            }
        };

        _channel.BasicConsume(destination, false, consumer);

        return Task.Delay(int.MaxValue, cancellationToken);
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }
}
