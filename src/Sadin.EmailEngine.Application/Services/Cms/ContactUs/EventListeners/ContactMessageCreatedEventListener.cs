using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sadin.Cms.Integration.Events.ContactUs;
using Sadin.EmailEngine.Application.Messaging.Models;
using Sadin.EmailEngine.Application.Services.Cms.ContactUs.EventHandlers;

namespace Sadin.EmailEngine.Application.Services.Cms.ContactUs.EventListeners;

public class ContactMessageCreatedEventListener : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ContactMessageCreatedEventListener(IOptions<MessagingOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        var messagingOptions = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = messagingOptions.HostName,
            Port = messagingOptions.HostPort,
            UserName = messagingOptions.UserName,
            Password = messagingOptions.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(exchange: "trigger",
            type: ExchangeType.Fanout);
        
        _queueName = _channel.QueueDeclare().QueueName;
        
        _channel.QueueBind(queue: _queueName,
            exchange: "trigger",
            routingKey: "");
    }


    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        stoppingToken.ThrowIfCancellationRequested();
        
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var contactMessageCreatedEvent = JsonSerializer.Deserialize<ContactMessageCreatedEvent>(message);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var contactMessageCreatedEventHandler =
                    scope.ServiceProvider.GetRequiredService<ContactMessageCreatedEventHandler>();
                
                await contactMessageCreatedEventHandler.Handle(contactMessageCreatedEvent);                
            }
        };

        _channel.BasicConsume(queue: _queueName,
            autoAck: true,
            consumer: consumer);
        
        return Task.CompletedTask;
    }
}