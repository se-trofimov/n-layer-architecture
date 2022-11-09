using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Messaging.Abstractions;
using RabbitMQ.Client;

namespace Messaging.Producer;

public class RabbitQueueProducer<TMessage> : IDisposable, IQueueProducer<TMessage>
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IBasicProperties _bp;

    public RabbitQueueProducer(IConnectionFactory connectionFactory)
    {
        IConnectionFactory connectionFactory1 = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _connection = connectionFactory1.CreateConnection();
        _channel = _connection.CreateModel();
        var bp = _channel.CreateBasicProperties();
        bp.ContentType = MediaTypeNames.Application.Json;
        _bp = bp;

    }

    public Task Send(TMessage message, string destination)
    {
        _channel.QueueDeclare(queue: destination, autoDelete: false, durable: true, exclusive: false);
        _channel.QueueBind(destination, "amq.direct", destination);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        return Task.Run(() =>
        {
            _channel.BasicPublish(exchange: "amq.direct",
                routingKey: destination,
                basicProperties: _bp,
                body: body);
        });
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }
}
