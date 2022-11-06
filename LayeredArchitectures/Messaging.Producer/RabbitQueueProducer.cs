using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Messaging.Abstractions;
using RabbitMQ.Client;

namespace Messaging.Producer;

public class RabbitQueueProducer<TMessage> : IDisposable, IQueueProducer<TMessage>
{
    private readonly Lazy<IConnection> _connection;
    private readonly Lazy<IModel> _channel;
    private readonly Lazy<IBasicProperties> _bp;

    public RabbitQueueProducer(IConnectionFactory connectionFactory)
    {
        IConnectionFactory connectionFactory1 = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _connection = new Lazy<IConnection>(() => connectionFactory1.CreateConnection());
        _channel = new Lazy<IModel>(() => _connection.Value.CreateModel());
        _bp = new Lazy<IBasicProperties>(() =>
        {
            var bp = _channel.Value.CreateBasicProperties();
            bp.ContentType = MediaTypeNames.Application.Json;
            return bp;
        });
    }

    public Task Send(TMessage message, string destination)
    {
        _channel.Value.QueueDeclare(destination);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        return Task.Run(() => _channel.Value.BasicPublish(exchange: "default",
             routingKey: "destination",
             basicProperties: _bp.Value,
             body: body));
    }

    public void Dispose()
    {
        _connection.Value.Dispose();
        _channel.Value.Dispose();
    }
}
