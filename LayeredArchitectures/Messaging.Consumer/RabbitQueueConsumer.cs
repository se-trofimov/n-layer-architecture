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
    private readonly Lazy<IConnection> _connection;
    private readonly Lazy<IModel> _channel;

    public RabbitQueueConsumer(IConnectionFactory connectionFactory, ILogger<RabbitQueueConsumer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        IConnectionFactory connectionFactory1 = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _connection = new Lazy<IConnection>(() => connectionFactory1.CreateConnection());
        _channel = new Lazy<IModel>(() => _connection.Value.CreateModel());
    }
    public Task ListenAsync<T>(string destination, Func<T, Task> onMessageReceived, CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel.Value);
        consumer.Received += (model, ea) =>
        {
            _logger.LogInformation("Message is received. Destination {destination}. ContentType {type}",
                destination, ea.BasicProperties.ContentType);

            IModel channel = model as IModel;
            if (ea.BasicProperties.ContentType != MediaTypeNames.Application.Json)
            {
                channel?.BasicAck(ea.DeliveryTag, false);
                return;
            }

            try
            {
                var body = ea.Body.Span;
                var deserialized = JsonSerializer.Deserialize<T>(body);

                if (deserialized is { })
                    onMessageReceived(deserialized);

                channel?.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                _logger.LogError("Unable process a message. Destination {destination}. Exception {exception}", 
                    destination, e);
            }
        };

        _channel.Value.BasicConsume(destination, false, consumer);

        return Task.Delay(int.MaxValue, cancellationToken);
    }
}
