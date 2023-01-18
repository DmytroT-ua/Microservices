using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Interface.RabbitMQ.v1;
using OrderApi.Messaging.Receive.Options.v1;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace B2B.InventorySyncApp.RabbitMQ
{
    public class RabbitMQSetupConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private readonly List<IModel> _channels = new List<IModel>();

        private readonly RabbitMqConfiguration _rabbitMqSettings;

        public RabbitMQSetupConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            var rabbitMqConfig = configuration.GetSection("RabbitMq");
            _rabbitMqSettings = rabbitMqConfig.Get<RabbitMqConfiguration>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _connection = GetConnection();

                var queueDeclareResult = new List<QueueDeclareOk>();

                #region Queues Declare
                foreach (RabbitQueue queue in _rabbitMqSettings.Queues)
                {
                    IModel channel = _connection.CreateModel();

                    QueueDeclareOk queueDeclare = channel.QueueDeclare(queue.Name, durable: true, exclusive: false, autoDelete: false);

                    queueDeclareResult.Add(queueDeclare);

                    foreach (RabbitBinding binding in queue.Bindings)
                    {
                        channel.ExchangeDeclare(binding.ExchangeName, binding.ExchangeType, durable: true);
                        foreach (string routingKey in binding.RoutingKeys)
                        {
                            channel.QueueBind(queue.Name, binding.ExchangeName, routingKey: routingKey);
                        }
                    }

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.Received += OnConsumerOnReceived;
                    consumer.Registered += OnConsumerOnRegistered;
                    consumer.Unregistered += OnConsumerOnUnregistered;
                    consumer.Shutdown += OnConsumerOnShutdown;
                    channel.BasicConsume(queue.Name, autoAck: false, consumer: consumer, consumerTag: GetConsumerTag(queue.Name));

                    _channels.Add(channel);
                }
                #endregion
            }
            catch (Exception ex)
            {
            }
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSettings.Hostname,
                UserName = _rabbitMqSettings.UserName,
                Password = _rabbitMqSettings.Password,
                Port = _rabbitMqSettings.Port,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(_rabbitMqSettings.NetworkRecoveryIntervalSecond)
            };

            return factory.CreateConnection();
        }

        private string GetConsumerTag(string queueName)
        {
            return $"{Environment.MachineName}_{Environment.UserName}_{queueName}";
        }

        private async Task OnConsumerOnReceived(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var rabbitMqListener = scope.ServiceProvider.GetService<IRabbitMQListener>();
                string message = Encoding.UTF8.GetString(@event.Body.ToArray());
                bool processSucceded = await rabbitMqListener.Subscribe(@event.RoutingKey, message);

                if (processSucceded)
                {
                    ((AsyncEventingBasicConsumer)sender).Model.BasicAck(@event.DeliveryTag, false);
                }
                else
                {
                    ((AsyncEventingBasicConsumer)sender).Model.BasicNack(@event.DeliveryTag, false, false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQConsumer subscribe error, ex; {ex.ToString()}");
            }
        }

        private Task OnConsumerOnRegistered(object sender, ConsumerEventArgs @event)
        {
            return Task.CompletedTask;
        }

        private Task OnConsumerOnUnregistered(object sender, ConsumerEventArgs @event)
        {
            return Task.CompletedTask;
        }

        private Task OnConsumerOnShutdown(object sender, ShutdownEventArgs @event)
        {
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channels.ForEach(z => z.Close());
            _channels.Clear();
            _connection?.Close();
            base.Dispose();
        }
    }
}

//#region Dead Letter Exchange declare
//IModel dlxChannel = _connection.CreateModel();

//QueueDeclareOk dlqDeclare = dlxChannel.QueueDeclare(
//    _rabbitMqSettings.DLQ.Name,
//    durable: true, exclusive: false, autoDelete: false);

//dlxChannel.ExchangeDeclare(
//    _rabbitMqSettings.DLQ.ExchangeName,
//    _rabbitMqSettings.DLQ.ExchangeType,
//    durable: true);

//dlxChannel.QueueBind(
//    _rabbitMqSettings.DLQ.Name,
//    _rabbitMqSettings.DLQ.ExchangeName,
//    _rabbitMqSettings.DLQ.RoutingKey);

//queueDeclareResult.Add(dlqDeclare);
//_channels.Add(dlxChannel);
//#endregion

//QueueDeclareOk queueDeclare = channel.QueueDeclare(queue.Name, durable: true, exclusive: false, autoDelete: false,
//                        arguments: new Dictionary<string, object> {
//                        { "x-dead-letter-exchange", _rabbitMqSettings.DLQ.ExchangeName },
//                        { "x-dead-letter-routing-key", _rabbitMqSettings.DLQ.RoutingKey }
//                    });